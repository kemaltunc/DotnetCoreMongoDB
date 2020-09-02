using System.Text.Json.Serialization;

namespace WallPaperApp.Dto.Base
{
    public class BaseResponse {
        [JsonPropertyName ("value")]
        public string Id { get; set; }
    }
}