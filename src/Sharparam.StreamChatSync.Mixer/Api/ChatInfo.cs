namespace Sharparam.StreamChatSync.Mixer.Api
{
    using Newtonsoft.Json;

    public class ChatInfo
    {
        [JsonProperty("endpoints")]
        public string[] Endpoints { get; set; }

        [JsonProperty("authkey")]
        public string Authkey { get; set; }

        [JsonProperty("permissions")]
        public string[] Permissions { get; set; }

        [JsonProperty("roles")]
        public string[] Roles { get; set; }
    }
}
