using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace WallPaperApp.Dto.Account
{
    public class RegisterRequest
    {
        [JsonPropertyName("email")] public string email { get; set; }

        [JsonPropertyName("name")] public string name { get; set; }

        [JsonPropertyName("surname")] public string surname { get; set; }
        [JsonPropertyName("password")] public string password { get; set; }
        [JsonPropertyName("path")] public string path { get; set; }
    }
}