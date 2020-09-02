using System.Threading.Tasks;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Base;
using WallPaperApp.Repository.Base;

namespace WallPaperApp.Repository.Account
{
    public interface IAccountRepository : IRepository<AccountEntity>
    {
    }
}