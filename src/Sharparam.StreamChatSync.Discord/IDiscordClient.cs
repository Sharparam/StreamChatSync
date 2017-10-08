namespace Sharparam.StreamChatSync.Discord
{
    using System;
    using System.Threading.Tasks;

    public interface IDiscordClient
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        Task StartAsync();

        Task StopAsync();

        Task SendAsync(string message);
    }
}
