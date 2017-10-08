namespace Sharparam.StreamChatSync.Mixer.Api
{
    using Newtonsoft.Json;

    public class Group
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
