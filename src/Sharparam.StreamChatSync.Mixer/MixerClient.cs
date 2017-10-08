namespace Sharparam.StreamChatSync.Mixer
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Api;

    using Configuration;

    using Data;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MixerClient : IMixerClient
    {
        private readonly ILogger _log;

        private readonly IMixerApi _api;

        private readonly MixerConfiguration _config;

        private readonly ILoggerFactory _loggerFactory;

        private ChatClient _chat;

        public MixerClient(ILogger<MixerClient> logger,
            IMixerApi api,
            IOptions<MixerConfiguration> options,
            ILoggerFactory loggerFactory)
        {
            _log = logger;
            _api = api;
            _config = options.Value;
            _loggerFactory = loggerFactory;
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public async Task Start()
        {
            _log.LogDebug("Getting Mixer user");
            var user = await _api.GetCurrentUserAsync();
            _log.LogInformation("Using mixer as {User}", user.Username);

            var chatInfo = await _api.GetChatInfoAsync(user.Channel.Id);

            _log.LogInformation("Got chat info: {@Info}", chatInfo);

            _log.LogDebug("Creating chat client");
            _chat = new ChatClient(_loggerFactory.CreateLogger<ChatClient>(),
                chatInfo.Endpoints.First(),
                chatInfo.Authkey,
                user);

            _chat.MessageReceived += ChatMessageReceived;

            _log.LogDebug("Connecting to chat");
            await _chat.ConnectAsync();
        }

        public async Task Stop()
        {
            await _chat.DisconnectAsync();
        }

        public async Task SendAsync(string message)
        {
            if (_chat != null)
                await _chat.SendAsync(message);
        }

        private void OnMessageReceived(MixerMessage message)
        {
            if (message.User == _config.Username && message.Content.StartsWith("[Discord]"))
                return;

            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
        }

        private void ChatMessageReceived(object sender, MixerMessage message)
        {
            OnMessageReceived(message);
        }
    }
}
