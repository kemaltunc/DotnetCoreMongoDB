using System;
using MongoDB.Bson.Serialization.Attributes;
using WallPaperApp.Dto.User;
using WallPaperApp.Entity.Base;

namespace WallPaperApp.Entity.Comment
{
    public class CommentEntity : BaseEntity
    {
        [BsonElement] public Object senderId { get; set; }
        [BsonElement] public string postId { get; set; }
        [BsonElement] public string content { get; set; }
    }
}