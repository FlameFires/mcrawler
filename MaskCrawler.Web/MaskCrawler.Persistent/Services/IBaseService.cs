using MaskCrawler.Models.Dto;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Services
{
    public interface IBaseService<TEntity>
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> Get(TEntity entity);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Get<TKey>(TKey id);

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <param name="predict"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predict);

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetList(object whereConditions);


        /// <summary>
        /// 分页获取实体集合
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetListPage(PageDto pageDto);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> Insert(TEntity entity);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TKey> Insert<TKey>(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Delete(TEntity entity);

        /// <summary>
        /// sql脚本执行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<int> Execute(string sql, object param = null, CommandType? commandType = null);
    }
}
