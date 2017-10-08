namespace Sharparam.StreamChatSync.Twitch
{
    using System;

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(TwitchMessage twitchMessage)
        {
            TwitchMessage = twitchMessage;
        }

        public TwitchMessage TwitchMessage { get; }
    }
}
