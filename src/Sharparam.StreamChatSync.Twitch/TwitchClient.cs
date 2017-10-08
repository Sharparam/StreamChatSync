namespace Sharparam.StreamChatSync.Twitch
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;

    using Configuration;

    using JetBrains.Annotations;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [UsedImplicitly]
    internal sealed class TwitchClient : ITwitchClient
    {
        /// <summary>
        /// A margin (in seconds) applied to the throttle period to make sure we are not throttled.
        /// </summary>
        private const double ThrottleMargin = 1.0;

        private readonly TwitchConfiguration _config;

        private readonly ILogger _log;

        private readonly ConcurrentQueue<string> _sendQueue;

        private TcpClient _client;

        private StreamReader _reader;

        private Timer _sendTimer;

        private Stream _stream;

        private StreamWriter _writer;

        public TwitchClient(ILogger<TwitchClient> logger, IOptions<TwitchConfiguration> options)
        {
            _log = logger;
            _config = options.Value;
            _sendQueue = new ConcurrentQueue<string>();
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public async Task ConnectAsync()
        {
            _client = new TcpClient();

            _sendTimer = new Timer((_config.ThrottlePeriod - ThrottleMargin) / _config.ThrottleCount)
            {
                AutoReset = false
            };
            _sendTimer.Elapsed += SendTimerElapsed;

            _log.LogDebug("Connecting to {Hostname}:{Port}", _config.Hostname, _config.Port);
            await _client.ConnectAsync(_config.Hostname, _config.Port).ContinueWith(ConnectComplete);
        }

        public void Disconnect()
        {
            _sendTimer?.Stop();
            _writer?.Dispose();
            _reader?.Dispose();
            _stream?.Dispose();
            _client?.Dispose();
        }

        public async Task SendAsync(string message)
        {
            await SendRawAsync($"PRIVMSG {_config.Channel} :{message}");
        }

        public async Task SendRawAsync(string content)
        {
            await QueueAsync(content);
        }

        private async Task ConnectComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
            {
                _log.LogCritical(task.Exception, "Failed to connect to Twitch servers");
                return;
            }

            _log.LogInformation("Successfully connected to Twitch servers");

            _log.LogDebug("Obtaining stream");
            _stream = _client.GetStream();

            _log.LogTrace("Creating reader");
            _reader = new StreamReader(_stream);
            _log.LogTrace("Creating writer");
            _writer = new StreamWriter(_stream, new UTF8Encoding(false)) {AutoFlush = true};

            _log.LogInformation("Sending authentication token");
            _log.LogDebug("Auth token: {Token}", _config.Token);
            await SendRawAsync($"PASS {_config.Token}");
            _log.LogDebug("Sending NICK command with nickname {Nick}", _config.Username);
            await SendRawAsync($"NICK {_config.Username}");

            _log.LogDebug("Calling reader asynchronously");
            await _reader.ReadLineAsync().ContinueWith(ReadComplete);
        }

        private async Task HandleMessage(ServerMessage message)
        {
            switch (message.Command)
            {
                case "001":
                case "002":
                case "003":
                case "004":
                case "375":
                case "372":
                    break;

                case "PING":
                    await SendRawAsync($"PONG :{message.Trailing}");
                    break;

                case "PRIVMSG":
                    var msg = new TwitchMessage(message.Parameters.First(),
                        message.Prefix.Split('!').First(),
                        message.Trailing);

                    if (msg.Sender == _config.Username)
                        break;

                    OnMessageReceived(msg);
                    break;

                case "376":
                    _log.LogInformation("Connection to IRC established, joining {Channel}", _config.Channel);
                    await SendRawAsync($"JOIN {_config.Channel}");
                    break;

                case "JOIN":
                    var user = message.Prefix.Split('!').First();
                    if (user == _config.Username)
                        _log.LogInformation("Successfully joined {Channel}", message.Parameters.First());
                    else
                        _log.LogTrace("{User} joined {Channel}", user, message.Parameters.First());
                    break;

                default:
                    _log.LogTrace("Unsupported command: {Command}", message.Command);
                    break;
            }
        }

        private void OnMessageReceived(TwitchMessage twitchMessage)
        {
            _log.LogTrace("Broadcasting {@Message} to event listeners", twitchMessage);
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(twitchMessage));
        }

        private Task QueueAsync(string content)
        {
            _sendQueue.Enqueue(content);
            if (_sendTimer.Enabled)
                return Task.CompletedTask;

            _log.LogTrace("Send timer stopped, starting it");
            _sendTimer.Start();
            return Task.CompletedTask;
        }

        private async Task ReadComplete(Task<string> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                var content = task.Result;
                _log.LogTrace(content);
                if (!string.IsNullOrEmpty(content))
                {
                    var message = ServerMessage.Parse(content);
                    _log.LogTrace("RCV: {@Message}", message);
                    await HandleMessage(message);
                }
            }
            else
            {
                _log.LogWarning("Failed to read line from Twitch server");
            }

            await _reader.ReadLineAsync().ContinueWith(ReadComplete);
        }

        private void SendTimerElapsed(object sender, ElapsedEventArgs args)
        {
            if (_sendQueue.IsEmpty)
                return;

            var success = _sendQueue.TryDequeue(out var message);

            if (success)
            {
                _log.LogTrace("SND: {Content}", message);
                _writer.WriteLineAsync(message).Wait();
                _log.LogTrace("Write complete");
            }
            else
            {
                _log.LogWarning("Failed to dequeue message from message queue");
            }

            _sendTimer.Start();
        }
    }
}
