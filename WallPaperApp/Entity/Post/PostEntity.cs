using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WallPaperApp.Dto.User;
using WallPaperApp.Entity.Base;

namespace WallPaperApp.Entity.Post
{
    public class PostEntity : BaseEntity
    {
        [BsonElement] public string path { get; set; }
        [BsonElement] public string deviceName { get; set; }
        [BsonElement] public string description { get; set; }
        [BsonElement] public ObjectId senderId { get; set; }
    }
}