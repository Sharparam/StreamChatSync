namespace Sharparam.StreamChatSync.Mixer.Api
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Method
    {
        [JsonConstructor]
        public Method(long id, string name, params object[] arguments)
        {
            Name = name;
            Arguments = arguments;
            Id = id;
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PacketType Type { get; } = PacketType.Method;

        [JsonProperty("method")]
        public string Name { get; }

        [JsonProperty("arguments")]
        public object[] Arguments { get; }

        [JsonProperty("id")]
        public long Id { get; }
    }
}
