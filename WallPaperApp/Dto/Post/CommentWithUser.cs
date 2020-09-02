using System;
using System.Text.Json.Serialization;
using WallPaperApp.Dto.Base;
using WallPaperApp.Dto.User;

namespace WallPaperApp.Dto.Post
{
    public class CommentWithUser : BaseResponse
    {
        [JsonPropertyName("content")] public string content { get; set; }
        [JsonPropertyName("owner")] public UserResponse user { get; set; }
        [JsonPropertyName("createdAt")] public DateTime createdAt { get; set; }
    }
}