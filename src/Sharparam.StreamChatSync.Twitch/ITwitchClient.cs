namespace Sharparam.StreamChatSync.Twitch
{
    using System;
    using System.Threading.Tasks;

    public interface ITwitchClient
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        Task ConnectAsync();

        void Disconnect();

        Task SendAsync(string message);

        Task SendRawAsync(string content);
    }
}
