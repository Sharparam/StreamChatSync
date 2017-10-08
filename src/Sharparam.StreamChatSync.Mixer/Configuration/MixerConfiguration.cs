namespace Sharparam.StreamChatSync.Mixer.Configuration
{
    public class MixerConfiguration
    {
        public string ApiUrl { get; set; } = "https://mixer.com/api/v1/";

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Token { get; set; }

        public string Username { get; set; }
    }
}
