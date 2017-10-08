namespace Sharparam.StreamChatSync.Mixer.Api
{
    using Newtonsoft.Json;

    public class Social
    {
        [JsonProperty("steam")]
        public string Steam { get; set; }

        [JsonProperty("verified")]
        public string[] Verified { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("youtube")]
        public string Youtube { get; set; }
    }
}
