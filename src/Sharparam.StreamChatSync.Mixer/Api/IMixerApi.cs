namespace Sharparam.StreamChatSync.Mixer.Api
{
    using System.Threading.Tasks;

    public interface IMixerApi
    {
        Task<User> GetCurrentUserAsync();

        Task<ChatInfo> GetChatInfoAsync(long channelId);
    }
}
