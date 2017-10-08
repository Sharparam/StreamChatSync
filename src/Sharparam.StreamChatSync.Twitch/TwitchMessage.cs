namespace Sharparam.StreamChatSync.Twitch
{
    public class TwitchMessage
    {
        internal TwitchMessage(string channel, string sender, string content)
        {
            Channel = channel;
            Sender = sender;
            Content = content;
        }

        public string Channel { get; }

        public string Sender { get; }

        public string Content { get; }
    }
}
