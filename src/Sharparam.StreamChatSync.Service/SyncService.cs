namespace Sharparam.StreamChatSync.Service
{
    using System;

    using global::Discord;

    using Microsoft.Extensions.Logging;

    using Mixer;

    using PeterKottas.DotNetCore.WindowsService.Interfaces;

    using Twitch;

    using IDiscordClient = Discord.IDiscordClient;
    using MessageReceivedEventArgs = Twitch.MessageReceivedEventArgs;

    public class SyncService : IMicroService
    {
        private readonly ILogger _log;

        private readonly IDiscordClient _discord;

        private readonly ITwitchClient _twitch;

        private readonly IMixerClient _mixer;

        public SyncService(ILogger<SyncService> logger, IDiscordClient discord, ITwitchClient twitch, IMixerClient mixer)
        {
            _log = logger;
            _discord = discord;
            _discord.MessageReceived += DiscordMessageReceived;
            _twitch = twitch;
            _twitch.MessageReceived += TwitchMessageReceived;
            _mixer = mixer;
            _mixer.MessageReceived += MixerMessageReceived;
        }

        public void Start()
        {
            _log.LogInformation("Service is starting");
            _log.LogInformation("Connecting to discord");
            _discord.StartAsync().Wait();
            _log.LogInformation("Connected to discord!");
            _log.LogInformation("Connecting to twitch");
            _twitch.ConnectAsync().Wait();
            _log.LogInformation("Connected to twitch!");
            _log.LogInformation("Connecting to mixer");
            _mixer.Start();
            _log.LogInformation("Connected to mixer!");
        }

        public void Stop()
        {
            _log.LogInformation("Service is stopping");
            _discord.StopAsync().Wait();
            _twitch.Disconnect();
            _mixer.Stop();
        }

        private void DiscordMessageReceived(object sender, Discord.MessageReceivedEventArgs args)
        {
            var message = args.Message;
            _log.LogInformation("[Discord] {Channel} <{User}> {Message}",
                message.Channel,
                message.User,
                message.Content);
            _twitch.SendAsync($"[Discord] <{message.User}> {message.Content}");
            _mixer.SendAsync($"[Discord] <{message.User}> {message.Content}");
        }

        private void TwitchMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            var message = args.TwitchMessage;
            _log.LogInformation("[Twitch] {Channel} <{User}> {Message}",
                message.Channel,
                message.Sender,
                message.Content);
            _discord.SendAsync($"[Twitch] **<{message.Sender}>** {message.Content}");
            _mixer.SendAsync($"[Twitch] <{message.Sender}> {message.Content}");
        }

        private void MixerMessageReceived(object sender, Mixer.Data.MessageReceivedEventArgs args)
        {
            var message = args.Message;
            _log.LogInformation("[Mixer] {Channel} <{User}> {Message}", message.Channel, message.User, message.Content);
            _discord.SendAsync($"[Mixer] **<{message.User}>** {message.Content}");
            _twitch.SendAsync($"[Mixer] <{message.User}> {message.Content}");
        }
    }
}
