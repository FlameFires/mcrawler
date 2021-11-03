using MaskCrawler.Models.Dto;
using MaskCrawler.Persistent.Repositories;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Services.Implements
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {
        IBaseRepository<TEntity> baseRepository;

        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            this.baseRepository = baseRepository;
        }
        public virtual Task<TEntity> Get(TEntity entity) => baseRepository.Get(entity);
        public virtual Task<TEntity> Get<TKey>(TKey id) => baseRepository.Get(id);

        public virtual Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predict) => baseRepository.GetList(predict);

        public virtual Task<bool> Delete(TEntity entity) => baseRepository.Delete(entity);

        public virtual Task<int> Insert(TEntity entity) => baseRepository.Insert(entity);

        public virtual Task<TKey> Insert<TKey>(TEntity entity) => baseRepository.Insert<TKey>(entity);

        public virtual Task<bool> Update(TEntity entity) => baseRepository.Update(entity);

        public virtual Task<IEnumerable<TEntity>> GetList(object whereConditions) => baseRepository.GetList(whereConditions);

        public virtual Task<IEnumerable<TEntity>> GetListPage(PageDto pageDto) => baseRepository.GetListPage(pageDto.PageNumber, pageDto.RowsPerPage, pageDto.Conditions, pageDto.Orderby, pageDto.Parameters);

        public virtual Task<int> Execute(string sql, object param = null, CommandType? commandType = null) => baseRepository.Execute(sql, param, null, commandType);
    }
}
