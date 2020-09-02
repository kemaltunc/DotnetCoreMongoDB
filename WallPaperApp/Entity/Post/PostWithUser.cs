using System;
using WallPaperApp.Entity.Account;

namespace WallPaperApp.Entity.Post
{
    public class PostWithUser : PostEntity
    {
        public AccountEntity[] ownerUser { get; set; }
    }
}