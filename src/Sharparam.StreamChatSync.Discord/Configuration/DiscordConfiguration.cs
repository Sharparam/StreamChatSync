namespace Sharparam.StreamChatSync.Discord.Configuration
{
    using System.Collections.Generic;

    public class DiscordConfiguration
    {
        public ulong UserId { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public ulong ChannelId { get; set; }

        public ulong OwnerId { get; set; }

        public List<ulong> BlockedIds { get; set; } = new List<ulong>(0);
    }
}
