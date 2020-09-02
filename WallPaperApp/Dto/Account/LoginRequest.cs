using System.Text.Json.Serialization;

namespace WallPaperApp.Dto.Account
{
    public class LoginRequest
    {
        [JsonPropertyName("email")] public string email { get; set; }
        [JsonPropertyName("password")] public string password { get; set; }
    }
}