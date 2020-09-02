using System.Text.Json.Serialization;
using WallPaperApp.Dto.Base;

namespace WallPaperApp.Dto.User
{
    public class UserResponse : BaseResponse
    {
        [JsonPropertyName("name")] public string name { get; set; }
        [JsonPropertyName("surname")] public string surname { get; set; }
        [JsonPropertyName("path")] public string path { get; set; }
    }
}