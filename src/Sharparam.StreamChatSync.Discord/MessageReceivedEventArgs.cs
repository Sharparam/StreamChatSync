namespace Sharparam.StreamChatSync.Discord
{
    public class MessageReceivedEventArgs
    {
        public MessageReceivedEventArgs(DiscordMessage message)
        {
            Message = message;
        }

        public DiscordMessage Message { get; }
    }
}
