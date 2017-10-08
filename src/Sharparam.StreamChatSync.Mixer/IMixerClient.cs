namespace Sharparam.StreamChatSync.Mixer
{
    using System;
    using System.Threading.Tasks;

    using Data;

    public interface IMixerClient
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        Task Start();

        Task Stop();

        Task SendAsync(string message);
    }
}
