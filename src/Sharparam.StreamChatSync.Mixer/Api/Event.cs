namespace Sharparam.StreamChatSync.Mixer.Api
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Event
    {
        [JsonConstructor]
        public Event(string name, Dictionary<string, object> data)
        {
            Name = name;
            Data = data;
        }

        [JsonProperty("event")]
        public string Name { get; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; }
    }
}
