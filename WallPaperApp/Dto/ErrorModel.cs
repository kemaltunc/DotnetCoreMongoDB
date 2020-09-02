using System.Text.Json.Serialization;

namespace WallPaperApp.Dto {
    public class ErrorModel {
        [JsonPropertyName ("message")]
        public string message { get; set; }
    }
}