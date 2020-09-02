using System.Text.Json.Serialization;
using WallPaperApp.Dto.Base;

namespace WallPaperApp.Dto.Account
{
    public class AccountResponse : BaseResponse
    {
        [JsonPropertyName("email")] public string email { get; set; }

        [JsonPropertyName("name")] public string name { get; set; }

        [JsonPropertyName("surname")] public string surname { get; set; }
        
        [JsonPropertyName("path")] public string path { get; set; }

        [JsonPropertyName("accessToken")] public string accessToken { get; set; }

        [JsonPropertyName("refreshToken")] public string refreshToken { get; set; }
 
    }
}