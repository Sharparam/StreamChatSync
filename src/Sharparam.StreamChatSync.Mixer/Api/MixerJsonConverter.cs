namespace Sharparam.StreamChatSync.Mixer.Api
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class MixerJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartArray:
                    return serializer.Deserialize<object[]>(reader);

                case JsonToken.StartObject:
                    return serializer.Deserialize<Dictionary<string, object>>(reader);

                case JsonToken.Boolean:
                    return serializer.Deserialize<bool>(reader);

                case JsonToken.String:
                    return serializer.Deserialize<string>(reader);

                case JsonToken.Integer:
                    return serializer.Deserialize<long>(reader);

                case JsonToken.Float:
                    return serializer.Deserialize<double>(reader);

                case JsonToken.Date:
                    return serializer.Deserialize<DateTime>(reader);

                case JsonToken.Null:
                    return null;

                default:
                    throw new NotSupportedException($"{reader.TokenType} is not supported");
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }
    }
}
