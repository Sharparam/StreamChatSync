namespace Sharparam.StreamChatSync.Discord
{
    public class DiscordMessage
    {
        public DiscordMessage(string channel, string user, string content)
        {
            Channel = channel;
            User = user;
            Content = content;
        }

        public string Channel { get; }

        public string User { get; }

        public string Content { get; }
    }
}
