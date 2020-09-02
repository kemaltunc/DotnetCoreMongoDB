using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using WallPaperApp.Dto.Post;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Post;
using WallPaperApp.Repository.Base;

namespace WallPaperApp.Repository.Post
{
    public interface IPostRepository : IRepository<PostEntity>
    {
        Task<List<PostWithUser>> GetAll(int page, int pageSize = 10);
        Task<PostWithUser> Get(string postId);

        Task<List<PostWithUser>> GetAllWithUserId(int page, string userId, int pageSize = 10);
        Task<List<PostResponse>> LikedPost(int page, ObjectId userId, int pageSize = 10);
    }
}