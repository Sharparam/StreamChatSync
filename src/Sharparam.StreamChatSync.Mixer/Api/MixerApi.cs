namespace Sharparam.StreamChatSync.Mixer.Api
{
    using System;
    using System.Threading.Tasks;

    using Rest;

    public class MixerApi : IMixerApi, IDisposable
    {
        private const string CurrentUserResource = "users/current";

        private const string ChatsResource = "chats";

        private readonly IRestClient _client;

        public MixerApi(IRestClient client)
        {
            _client = client;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var user = await _client.GetAsync<User>(CurrentUserResource);
            return user;
        }

        public async Task<ChatInfo> GetChatInfoAsync(long channelId)
        {
            var info = await _client.GetAsync<ChatInfo>($"{ChatsResource}/{channelId}");
            return info;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
