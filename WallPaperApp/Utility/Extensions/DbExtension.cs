using System.Collections.Generic;
using MongoDB.Driver;
using WallPaperApp.Dto.Post;
using WallPaperApp.Dto.User;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Base;
using WallPaperApp.Entity.Post;

namespace WallPaperApp.Utility.Extensions
{
    public static class DbExtension
    {
        public static IMongoCollection<T> GetCollectionExt<T>(this IMongoDatabase db)
        {
            var collection = db.GetCollection<T>(typeof(T).Name.Substring(0, typeof(T).Name.Length - 6));
            return collection;
        }

    }
}