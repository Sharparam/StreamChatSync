namespace Sharparam.StreamChatSync.Mixer.Api
{
    using Newtonsoft.Json;

    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }

        [JsonProperty("channel")]
        public Channel Channel { get; set; }

        [JsonProperty("experience")]
        public long Experience { get; set; }

        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }

        [JsonProperty("groups")]
        public Group[] Groups { get; set; }

        [JsonProperty("sparks")]
        public long Sparks { get; set; }

        [JsonProperty("primaryTeam")]
        public object PrimaryTeam { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("social")]
        public Social Social { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}
