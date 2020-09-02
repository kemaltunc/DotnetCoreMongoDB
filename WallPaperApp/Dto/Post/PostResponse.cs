using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WallPaperApp.Dto.Account;
using WallPaperApp.Dto.Base;
using WallPaperApp.Dto.User;

namespace WallPaperApp.Dto.Post
{
    public class PostResponse : BaseResponse
    {
        [JsonPropertyName("path")] public string path { get; set; }

        [JsonPropertyName("deviceName")] public string deviceName { get; set; }

        [JsonPropertyName("description")] public string description { get; set; }

        [JsonPropertyName("owner")] public UserResponse ownerUser { get; set; }


        [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }


        [JsonPropertyName("likeCount")] public int LikeCount { get; set; }
        [JsonPropertyName("commentCount")] public int CommentCount { get; set; }

        [JsonPropertyName("isLike")] public bool isLike { get; set; }
    }
}