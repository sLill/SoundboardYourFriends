using Newtonsoft.Json;

namespace SoundboardYourFriends.Update.Data
{
    public class Asset
    {
        #region Properties..
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
        #endregion Properties..
    }
}
