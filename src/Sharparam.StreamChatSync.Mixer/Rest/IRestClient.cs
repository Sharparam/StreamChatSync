namespace Sharparam.StreamChatSync.Mixer.Rest
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IRestClient : IDisposable
    {
        Task<T> GetAsync<T>(string resource, object data = null);

        Task<T> RequestAsync<T>(HttpMethod method, string resource, object data = null);

        Task<HttpResponseMessage> RequestAsync(HttpMethod method, string resource, object data = null);
    }
}
