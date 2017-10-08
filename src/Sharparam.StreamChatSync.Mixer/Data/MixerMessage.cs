namespace Sharparam.StreamChatSync.Mixer.Data
{
    public class MixerMessage
    {
        public MixerMessage(string user, string channel, string content)
        {
            User = user;
            Channel = channel;
            Content = content;
        }

        public string User { get; }

        public string Channel { get; }

        public string Content { get; }
    }
}
