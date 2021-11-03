using MaskCrawler.Http;
using MaskCrawler.Models.Domain;
using MaskCrawler.Models.Dto;
using MaskCrawler.Models.Http;
using MaskCrawler.Persistent.Repositories;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Services.Implements
{
    /// <summary>
    /// 生成JWT的service类
    /// </summary>
    public class TaskService : BaseService<TaskEntity>, ITaskService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IHttpDecorator httpDecorator;
        public TaskService(ITaskRepository taskRepository, IHttpDecorator httpDecorator) : base(taskRepository)
        {
            this.taskRepository = taskRepository;
            this.httpDecorator = httpDecorator;
        }

        public async Task<IActionResult> Add(TaskInfoDto dto)
        {
            var entity = new TaskEntity
            {
                AccountId = dto.AccountGid,
                TaskName = dto.TaskName,
                Url = dto.Url,
                Method = dto.Method.ToString(),
                Header = dto.Header,
                ResolveType = dto.ResolveType.ToString(),
                ResolvePattern = dto.ResolvePattern
            }.Init();

            var tid = await Insert<Guid>(entity);
            var n = tid != default(Guid) ? 1 : 0;
            return BackResult.Judge(n, failMsg: "添加失败");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == default) return BackResult.Failed("错误参数");

            var task = await Get<Guid>(id);
            if (task == null) return BackResult.Failed("没获取到任务信息");

            var num = await Delete(task);
            return BackResult.Judge(num, failMsg: "删除失败");
        }

        public Task<IActionResult> Pause(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Start(Guid id)
        {
            if (id == default) return BackResult.Failed("错误参数");

            var entity = await Get<Guid>(id);
            if (entity == null) return BackResult.Failed("没获取到任务信息");

            var info = new HttpInfo
            {
                Url = entity.Url,
                Header = entity.Header,
                Method = new System.Net.Http.HttpMethod(entity.Method.ToString())
            };

            var str = await httpDecorator.ReqString(info);
            return BackResult.Successed(data: str);
        }

        public async Task<IActionResult> Query(TaskQueryDto queryDto)
        {
            if (!string.IsNullOrWhiteSpace(queryDto.Key))
                queryDto.Conditions = String.Format("where task.name like '%{0}%' or task.describe like '%{0}%' or url like '%{0}%'", queryDto.Key);


            StringBuilder orderby = new StringBuilder("createDate desc");
            if (queryDto.Orderbys != null)
                foreach (var item in queryDto.Orderbys)
                {
                    orderby.Append($",{item.Key} {item.type.ToString()}");
                }
            queryDto.Orderby = orderby.ToString();


            var list = await GetListPage(queryDto);
            return BackResult.Judge(list, failMsg: "获取失败");
        }

        public async Task<IActionResult> Aquire(Guid id) => BackResult.Judge(await taskRepository.Get(id), failMsg: "获取失败");

        public async Task<IActionResult> Update(TaskInfoDto dto)
        {
            var entity = new TaskEntity
            {
                TaskId = dto.TaskId,
                AccountId = dto.AccountGid,
                TaskName = dto.TaskName,
                Url = dto.Url,
                Method = dto.Method.ToString(),
                Header = dto.Header,
                ResolveType = dto.ResolveType.ToString(),
                ResolvePattern = dto.ResolvePattern
            };

            var sql = new StringBuilder("update task set ");
            sql.Append($" name=@TaskName, ");
            //sql.Append($" describe=@Desc, ");
            sql.Append($" url=@Url, ");
            sql.Append($" method=@Method, ");
            sql.Append($" header=@Header, ");
            sql.Append($" rType=@ResolveType, ");
            sql.Append($" rPattern=@ResolvePattern ");
            sql.Append($" where tid=@TaskId");

            var j = await Execute(sql.ToString(), dto);
            return BackResult.Judge(j, failMsg: "修改失败");
        }
    }
}
