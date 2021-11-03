using MaskCrawler.Models.Domain;
using MaskCrawler.Persistent.Infrastructure;

namespace MaskCrawler.Persistent.Repositories.Implements
{
    public class AccountRepository : BaseRepository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(IDatabaseAdapter database) : base(database)
        {
        }
    }
}
