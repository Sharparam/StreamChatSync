namespace Sharparam.StreamChatSync.Discord
{
    using System;
    using System.Threading.Tasks;

    using Configuration;

    using global::Discord;
    using global::Discord.WebSocket;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class DiscordClient : IDiscordClient
    {
        private readonly DiscordSocketClient _client;

        private readonly DiscordConfiguration _config;

        private readonly ILogger _discordLog;

        private readonly ILogger _log;

        private SocketTextChannel _channel;

        public DiscordClient(ILogger<DiscordClient> logger,
            IOptions<DiscordConfiguration> options,
            ILogger<DiscordSocketClient> discordLogger)
        {
            _log = logger;
            _discordLog = discordLogger;
            _config = options.Value;

            _log.LogDebug("Creating new Discord socket client");
            _client = new DiscordSocketClient();
            _client.Log += DiscordClientLog;
            _client.MessageReceived += DiscordMessageReceived;
            _client.Connected += DiscordConnected;
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public async Task StartAsync()
        {
            _log.LogDebug("Logging in");
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            _log.LogDebug("Starting");
            await _client.StartAsync();
        }

        public async Task StopAsync()
        {
            await _client.StopAsync();
            _client?.Dispose();
        }

        public async Task SendAsync(string message)
        {
            if (_channel == null && (_channel = _client.GetChannel(_config.ChannelId) as SocketTextChannel) == null)
            {
                _log.LogWarning("Unable to send message, channel not initialised");
                return;
            }

            await _channel.SendMessageAsync(message);
        }

        private Task DiscordConnected()
        {
            _log.LogInformation("Discord connected");
            _log.LogDebug("Getting channel");
            var socketChannel = _client.GetChannel(_config.ChannelId);
            _channel = socketChannel as SocketTextChannel;
            return Task.CompletedTask;
        }

        private Task DiscordMessageReceived(SocketMessage message)
        {
            if (message.Channel.Id != _config.ChannelId)
                return Task.CompletedTask;

            _log.LogDebug("{Channel} {Author} {Content}",
                message.Channel.Name,
                message.Author.Username,
                message.Content);

            if (message.Author.Id == _config.UserId || _config.BlockedIds.Contains(message.Author.Id))
                return Task.CompletedTask;

            var user = message.Author.Id == _config.OwnerId
                ? $"@{message.Author.Username}"
                : $"{message.Author.Username}#{message.Author.DiscriminatorValue}";

            OnMessageReceived(new DiscordMessage(message.Channel.Name, user, message.Content));

            return Task.CompletedTask;
        }

        private void OnMessageReceived(DiscordMessage message)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
        }

        private Task DiscordClientLog(LogMessage entry)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (entry.Severity)
            {
                case LogSeverity.Critical:
                    _discordLog.LogCritical(entry.Exception,
                        "{Source} - {Message}",
                        entry.Source,
                        entry.Message);
                    break;

                case LogSeverity.Error:
                    _discordLog.LogError(entry.Exception,
                        "{Source} - {Message}",
                        entry.Source,
                        entry.Message);
                    break;

                case LogSeverity.Warning:
                    _discordLog.LogWarning(entry.Exception,
                        "{Source} - {Message}",
                        entry.Source,
                        entry.Message);
                    break;

                case LogSeverity.Info:
                    _discordLog.LogInformation(entry.Exception,
                        "{Source} - {Message}",
                        entry.Source,
                        entry.Message);
                    break;

                case LogSeverity.Verbose:
                    _discordLog.LogTrace(entry.Exception,
                        "{Source} - {Message}",
                        entry.Source,
                        entry.Message);
                    break;

                case LogSeverity.Debug:
                    _discordLog.LogDebug(entry.Exception,
                        "{Source} - {Message}",
                        entry.Source,
                        entry.Message);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
