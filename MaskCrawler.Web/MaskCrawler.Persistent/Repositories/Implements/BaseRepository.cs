

using Dapper;

using Dommel;

using MaskCrawler.Models;
using MaskCrawler.Persistent.Infrastructure;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Repositories.Implements
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable
    {
        protected BaseRepository(IDatabaseAdapter database)
        {
            this.database = database;
            this.db = database.GetConnection();
            this.dbConfig = database.DbConfig;
        }

        public IDatabaseAdapter database { get; set; }

        protected IDbConnection db { get; set; }

        protected DbConfig dbConfig { get; set; }

        public IDbTransaction GetTransaction() => db.BeginTransaction();

        public async Task<bool> Delete(TEntity entity, IDbTransaction transaction = null) => (await db.DeleteAsync(entity, commandTimeout: dbConfig.Timeout)) > 0;

        public async Task<TEntity> Get<TKeyType>(TKeyType id, IDbTransaction transaction = null) => await db.GetAsync<TEntity>(id, commandTimeout: dbConfig.Timeout);

        public async Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predict, IDbTransaction transaction = null) => await db.SelectAsync<TEntity>(predict, transaction);

        public async Task<IEnumerable<TEntity>> GetList(object whereConditions, IDbTransaction transaction = null) => await db.GetListAsync<TEntity>(whereConditions, transaction, dbConfig.Timeout);

        public async Task<IEnumerable<TEntity>> GetListPage(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null, IDbTransaction transaction = null) => await db.GetListPagedAsync<TEntity>(pageNumber, rowsPerPage, conditions, orderby, parameters, transaction, dbConfig.Timeout);

        public async Task<int> Insert(TEntity entity, IDbTransaction transaction = null) => (await db.InsertAsync(entity, transaction, commandTimeout: dbConfig.Timeout)).Value;

        public async Task<TKey> Insert<TKey>(TEntity entity, IDbTransaction transaction = null) => await db.InsertAsync<TKey, TEntity>(entity, transaction, commandTimeout: dbConfig.Timeout);

        public async Task<bool> Update(TEntity entity, IDbTransaction transaction = null) => (await db.UpdateAsync(entity, commandTimeout: dbConfig.Timeout)) > 0 ? true : false;
        public async Task<int> Execute(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null) => await db.ExecuteAsync(sql, param, transaction, dbConfig.Timeout, commandType);

        public void Dispose()
        {
            if (db != null && db.State != ConnectionState.Closed)
            {
                db.Dispose();
                db = null;
            }
        }
    }
}
