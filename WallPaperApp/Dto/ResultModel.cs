using System.Text.Json.Serialization;

namespace WallPaperApp.Dto {
    public class ResultModel {
        [JsonPropertyName ("result")]
        public bool result { get; set; }

    }
}