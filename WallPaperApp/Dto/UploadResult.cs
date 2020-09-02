using System.Text.Json.Serialization;

namespace WallPaperApp.Dto {
    public class UploadResult {
        [JsonPropertyName ("path")]
        public string path { get; set; }
    }
}