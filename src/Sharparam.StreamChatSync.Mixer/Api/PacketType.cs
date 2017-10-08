namespace Sharparam.StreamChatSync.Mixer.Api
{
    using System.Runtime.Serialization;

    public enum PacketType
    {
        [EnumMember(Value = "method")]
        Method,

        [EnumMember(Value = "reply")]
        Reply,

        [EnumMember(Value = "event")]
        Event
    }
}
