namespace Sharparam.StreamChatSync.Mixer.Rest
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Configuration;

    using Microsoft.Extensions.Options;

    using Newtonsoft.Json;

    public class RestClient : IRestClient
    {
        private readonly HttpClient _client;

        public RestClient(IOptions<MixerConfiguration> options)
        {
            var config = options.Value;

            _client = new HttpClient
            {
                BaseAddress = new Uri(config.ApiUrl),
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json")},
                    Authorization = new AuthenticationHeaderValue("Bearer", config.Token)
                }
            };
        }

        public async Task<T> GetAsync<T>(string resource, object data = null)
        {
            return await RequestAsync<T>(HttpMethod.Get, resource, data);
        }

        public async Task<T> RequestAsync<T>(HttpMethod method, string resource, object data = null)
        {
            var response = await RequestAsync(method, resource, data);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<HttpResponseMessage> RequestAsync(HttpMethod method, string resource, object data = null)
        {
            var request = new HttpRequestMessage(method, resource);

            if (data != null)
            {
                var content = new StringContent(JsonConvert.SerializeObject(data));
                request.Content = content;
                request.Headers.Add("Content-Type", "application/json");
            }

            return await _client.SendAsync(request);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
