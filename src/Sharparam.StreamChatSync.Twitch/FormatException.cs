namespace Sharparam.StreamChatSync.Twitch
{
    using System;

    public class FormatException : Exception
    {
        public FormatException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
