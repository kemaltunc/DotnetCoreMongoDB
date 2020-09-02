using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WallPaperApp.Dto.User;
using WallPaperApp.Entity.Base;

namespace WallPaperApp.Entity.Like
{
    public class LikeEntity : BaseEntity
    {
        
        [BsonElement] public ObjectId postId { get; set; }
        [BsonElement] public ObjectId userId { get; set; }
    }
}