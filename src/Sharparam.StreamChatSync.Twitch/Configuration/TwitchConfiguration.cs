namespace Sharparam.StreamChatSync.Twitch.Configuration
{
    public class TwitchConfiguration
    {
        public string Hostname { get; set; } = "irc.chat.twitch.tv";

        public ushort Port { get; set; } = 443;

        public bool UseSsl { get; set; } = true;

        public string Token { get; set; }

        public string Channel { get; set; }

        public string Username { get; set; }

        /// <summary>
        /// Throttle period, in seconds. Default is 30 seconds for both non-mods and mods.
        /// </summary>
        public double ThrottlePeriod { get; set; } = 30.0;

        /// <summary>
        /// Throttle message limit. Default for non-mods is 20 seconds, 100 for mods.
        /// </summary>
        public int ThrottleCount { get; set; } = 100;
    }
}
