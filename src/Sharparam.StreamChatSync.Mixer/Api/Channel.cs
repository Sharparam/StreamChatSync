namespace Sharparam.StreamChatSync.Mixer.Api
{
    using Newtonsoft.Json;

    public class Channel
    {
        [JsonProperty("interactive")]
        public bool Interactive { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("costreamId")]
        public object CostreamId { get; set; }

        [JsonProperty("badgeId")]
        public object BadgeId { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("bannerUrl")]
        public string BannerUrl { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("coverId")]
        public long CoverId { get; set; }

        [JsonProperty("deletedAt")]
        public object DeletedAt { get; set; }

        [JsonProperty("hasTranscodes")]
        public bool HasTranscodes { get; set; }

        [JsonProperty("featured")]
        public bool Featured { get; set; }

        [JsonProperty("featureLevel")]
        public long FeatureLevel { get; set; }

        [JsonProperty("ftl")]
        public long Ftl { get; set; }

        [JsonProperty("hosteeId")]
        public object HosteeId { get; set; }

        [JsonProperty("hasVod")]
        public bool HasVod { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("thumbnailId")]
        public object ThumbnailId { get; set; }

        [JsonProperty("numFollowers")]
        public long NumFollowers { get; set; }

        [JsonProperty("languageId")]
        public object LanguageId { get; set; }

        [JsonProperty("interactiveGameId")]
        public object InteractiveGameId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("partnered")]
        public bool Partnered { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("suspended")]
        public bool Suspended { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("transcodingProfileId")]
        public long TranscodingProfileId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("typeId")]
        public long TypeId { get; set; }

        [JsonProperty("viewersCurrent")]
        public long ViewersCurrent { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("viewersTotal")]
        public long ViewersTotal { get; set; }

        [JsonProperty("vodsEnabled")]
        public bool VodsEnabled { get; set; }
    }
}
