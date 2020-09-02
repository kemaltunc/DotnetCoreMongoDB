using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WallPaperApp.Dto.Post;
using WallPaperApp.Entity.Comment;
using WallPaperApp.Entity.Like;
using WallPaperApp.Repository.Base;

namespace WallPaperApp.Repository.Comment
{
    public interface ICommentRepository : IRepository<CommentEntity>
    {
        Task<List<CommentWithUser>> Get(string postId);
    }
}