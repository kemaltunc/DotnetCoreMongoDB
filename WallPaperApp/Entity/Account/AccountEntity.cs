using MongoDB.Bson.Serialization.Attributes;
using WallPaperApp.Entity.Base;

namespace WallPaperApp.Entity.Account
{
    public class AccountEntity : BaseEntity
    {
        [BsonElement] public string email { get; set; }
        [BsonElement] public string name { get; set; }
        [BsonElement] public string surname { get; set; }
        [BsonElement] public string password { get; set; }
        [BsonElement] public string path { get; set; }
    }
}