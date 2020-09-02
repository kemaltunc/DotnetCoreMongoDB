using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Infrastructure;
using WallPaperApp.Repository.Base;

namespace WallPaperApp.Repository.Account
{
    public class AccountRepository : BaseRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(IOptions<MongoDbSettings> options) : base(options)
        {
        }
        
    }
}