namespace Sharparam.StreamChatSync.Mixer.Api
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Reply
    {
        [JsonConstructor]
        public Reply(string error, Dictionary<string, object> data, long id)
        {
            Error = error;
            Data = data;
            Id = id;
        }

        [JsonProperty("error")]
        public string Error { get; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; }

        [JsonProperty("id")]
        public long Id { get; }
    }
}
