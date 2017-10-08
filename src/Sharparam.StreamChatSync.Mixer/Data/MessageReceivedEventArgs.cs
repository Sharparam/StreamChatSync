namespace Sharparam.StreamChatSync.Mixer.Data
{
    using System;

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(MixerMessage message)
        {
            Message = message;
        }

        public MixerMessage Message { get; }
    }
}
