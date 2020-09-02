using Microsoft.Extensions.Options;
using WallPaperApp.Entity.Like;
using WallPaperApp.Infrastructure;
using WallPaperApp.Repository.Base;

namespace WallPaperApp.Repository.Like
{
    public class LikeRepository : BaseRepository<LikeEntity>, ILikeRepository
    {
        public LikeRepository(IOptions<MongoDbSettings> options) : base(options)
        {
            
        }
    }
}