using MaskCrawler.Models;

using Microsoft.Extensions.Configuration;

using System.Data;

namespace MaskCrawler.Persistent.Infrastructure
{
    public interface IDatabaseAdapter
    {
        IDbConnection Connection { get; }
        IConfiguration Configuration { get; }
        DbConfig DbConfig { get; }

        void InitDataBase();

        IDbConnection GetConnection();
    }
}
