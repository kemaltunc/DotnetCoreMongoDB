using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WallPaperApp.Dto.Post;
using WallPaperApp.Dto.User;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Like;
using WallPaperApp.Entity.Post;
using WallPaperApp.Infrastructure;
using WallPaperApp.Repository.Account;
using WallPaperApp.Repository.Base;
using WallPaperApp.Utility.Extensions;
using Enumerable = System.Linq.Enumerable;

namespace WallPaperApp.Repository.Post
{
    public class PostRepository : BaseRepository<PostEntity>, IPostRepository
    {
        public PostRepository(IOptions<MongoDbSettings> options) : base(options)
        {
        }

        public async Task<List<PostWithUser>> GetAll(int page, int pageSize = 10)
        {
            var posts = Db.GetCollectionExt<PostEntity>();
            var users = Db.GetCollectionExt<AccountEntity>();


            /*var query = await (from p in posts.AsQueryable()
                    join o in users.AsQueryable()
                        on p.senderId equals o.Id
                    select new PostResponse
                    {
                        Id = p.Id,
                        path = p.path,
                        deviceName = p.deviceName,
                        description = p.description,
                        CreatedAt = p.CreatedAt,
                        ownerUser = mapper.Map<UserResponse>(o)
                    }).OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();*/

            var result = await posts.Aggregate()
                .Lookup<PostEntity, AccountEntity, PostWithUser>(
                    users,
                    x => x.senderId,
                    y => y.Id,
                    x => x.ownerUser).SortByDescending(x => x.CreatedAt).Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();


            return result;
        }

        public async Task<PostWithUser> Get(string postId)
        {
            var posts = Db.GetCollectionExt<PostEntity>();
            var users = Db.GetCollectionExt<AccountEntity>();

            var result = await posts.Aggregate()
                .Match(x => x.Id == postId)
                .Lookup<PostEntity, AccountEntity, PostWithUser>(
                    users,
                    x => x.senderId,
                    y => y.Id,
                    x => x.ownerUser).FirstOrDefaultAsync();


            return result;
        }

        public async Task<List<PostWithUser>> GetAllWithUserId(int page, string userId, int pageSize = 10)
        {
            var posts = Db.GetCollectionExt<PostEntity>();
            var users = Db.GetCollectionExt<AccountEntity>();
            var result = await posts.Aggregate()
                .Match(x => x.senderId == new ObjectId(userId))
                .Lookup<PostEntity, AccountEntity, PostWithUser>(
                    users,
                    x => x.senderId,
                    y => y.Id,
                    x => x.ownerUser).SortByDescending(x => x.CreatedAt).Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();


            return result;
        }

        public async Task<List<PostResponse>> LikedPost(int page, ObjectId userId, int pageSize = 10)
        {
            var posts = Db.GetCollectionExt<PostEntity>();
            var users = Db.GetCollectionExt<AccountEntity>();
            var likes = Db.GetCollectionExt<LikeEntity>();
            

            var result = await (from l in likes.AsQueryable()
                    join u in users.AsQueryable() on (object) l.userId equals u.Id
                    join p in posts.AsQueryable() on (object) l.postId equals p.Id
                    where (l.userId == userId)
                    select new PostResponse()
                    {
                        Id = p.Id,
                        path = p.path,
                        deviceName = p.deviceName,
                        description = p.description,
                        CreatedAt = p.CreatedAt,
                        ownerUser = new UserResponse
                        {
                            Id = u.Id,
                            name = u.name,
                            surname = u.surname,
                            path = u.path
                        }
                    }
                ).OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }
    }
}