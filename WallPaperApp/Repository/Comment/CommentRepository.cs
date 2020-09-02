using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WallPaperApp.Dto.Post;
using WallPaperApp.Dto.User;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Base;
using WallPaperApp.Entity.Comment;
using WallPaperApp.Entity.Like;
using WallPaperApp.Infrastructure;
using WallPaperApp.Repository.Base;
using WallPaperApp.Repository.Like;
using WallPaperApp.Utility.Extensions;

namespace WallPaperApp.Repository.Comment
{
    public sealed class CommentRepository : BaseRepository<CommentEntity>, ICommentRepository
    {
        public CommentRepository(IOptions<MongoDbSettings> options, IMapper mapper) : base(options)
        {
        }


        public async Task<List<CommentWithUser>> Get(string postId)
        {
            var users = Db.GetCollectionExt<AccountEntity>();
            var comments = Db.GetCollectionExt<CommentEntity>();

            var query = await comments.AsQueryable()
                .Where(p => p.postId == postId)
                .GroupJoin(
                    users.AsQueryable(),
                    p => p.senderId,
                    o => o.Id,
                    (p, o) => new {p.content, p.CreatedAt, p.Id, other = o.First()}
                )
                .Select(x => new CommentWithUser()
                {
                    Id = x.Id,
                    content = x.content,
                    createdAt = x.CreatedAt,
                    user = new UserResponse
                    {
                        Id = x.other.Id,
                        name = x.other.name,
                        surname = x.other.surname,
                        path = x.other.path
                    }
                }).OrderByDescending(x => x.createdAt).ToListAsync();


            return query;
        }
    }
}