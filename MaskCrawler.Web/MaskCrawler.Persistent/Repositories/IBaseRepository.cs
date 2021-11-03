using MaskCrawler.Persistent.Infrastructure;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        IDatabaseAdapter database { get; set; }

        IDbTransaction GetTransaction();

        Task<TEntity> Get<TKeyType>(TKeyType id, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predict, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> GetList(object whereConditions, IDbTransaction transaction = null);

        Task<IEnumerable<TEntity>> GetListPage(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null, IDbTransaction transaction = null);

        Task<int> Insert(TEntity entity, IDbTransaction transaction = null);

        Task<TKey> Insert<TKey>(TEntity entity, IDbTransaction transaction = null);

        Task<bool> Update(TEntity entity, IDbTransaction transaction = null);

        Task<bool> Delete(TEntity entity, IDbTransaction transaction = null);

        Task<int> Execute(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null);

    }
}
