using MaskCrawler.Models.Domain;
using MaskCrawler.Models.Dto;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Services
{
    public interface ITaskService : IBaseService<TaskEntity>
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IActionResult> Aquire(Guid id);


        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IActionResult> Query(TaskQueryDto queryDto);

        /// <summary>
        /// 修改任务信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IActionResult> Update(TaskInfoDto queryDto);

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IActionResult> Add(TaskInfoDto dto);

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IActionResult> Start(Guid id);

        Task<IActionResult> Pause(Guid id);

        Task<IActionResult> Delete(Guid id);
    }
}
