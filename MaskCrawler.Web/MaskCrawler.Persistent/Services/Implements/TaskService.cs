using MaskCrawler.Http;
using MaskCrawler.Models.Authroize;
using MaskCrawler.Models.Domain;
using MaskCrawler.Models.Dto;
using MaskCrawler.Persistent.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
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
        private readonly IJwtService jwtService;
        public TaskService(ITaskRepository taskRepository, IHttpDecorator httpDecorator, IJwtService jwtService) : base(taskRepository)
        {
            this.taskRepository = taskRepository;
            this.httpDecorator = httpDecorator;
            this.jwtService = jwtService;
            httpDecorator.ErrorHandler += HttpDecorator_errorHandler;
        }

        public async Task<IActionResult> Add(TaskInfoDto dto)
        {
            var entity = new TaskEntity
            {
                AccountId = dto.AccountGid,
                TaskName = dto.TaskName,
                Url = dto.Url,
                Method = dto.Method ?? "GET",
                Header = dto.Header,
                ResolveType = dto.ResolveType,
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

            if (httpDecorator.StatusCode != -1)
            {
                return BackResult.Successed(data: str ?? "null");
            }
            else
            {
                return BackResult.Failed(httpDecorator.Exception.Message);
            }
        }

        private IActionResult HttpDecorator_errorHandler(Exception arg)
        {
            var msg = arg.Message;
            return BackResult.Failed(msg);
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


            IEnumerable<TaskEntity> list = await GetListPage(queryDto);
            return BackResult.Judge(list, failMsg: "获取失败");
        }

        public async Task<IActionResult> Aquire(Guid id) => BackResult.Judge(await taskRepository.Get(id), failMsg: "获取失败");

        public async Task<IActionResult> Update(TaskEntity entity)
        {
            //var entity = new TaskEntity
            //{
            //    TaskId = dto.TaskId,
            //    AccountId = dto.AccountGid,
            //    TaskName = dto.TaskName,
            //    TaskDescribe = dto.Desc,
            //    Url = dto.Url,
            //    Method = dto.Method.ToString(),
            //    Header = dto.Header,
            //    ResolveType = dto.ResolveType.ToString(),
            //    ResolvePattern = dto.ResolvePattern
            //};

            var sql = new StringBuilder("update task a set ");
            sql.Append($" name=@TaskName, ");
            sql.Append($" a.describe=@TaskDescribe, ");
            sql.Append($" url=@Url, ");
            sql.Append($" method=@Method, ");
            sql.Append($" header=@Header, ");
            sql.Append($" rType=@ResolveType, ");
            sql.Append($" rPattern=@ResolvePattern ");
            sql.Append($" where tid=@TaskId");

            var j = await Execute(sql.ToString(), entity);
            return BackResult.Judge(j, failMsg: "修改失败");
        }

        public async Task<IActionResult> Resolve(TaskEntity entity, HttpContext context, Func<TaskEntity, Task<IActionResult>> func)
        {
            // 判断是否登录，登录就记录下来
            Guid accountId = new Guid(jwtService.GetClaimValue(context, nameof(SessionEntity.Gid)).Value);

            //TaskEntity entity = ReflectUtil.Convert<TaskEntity>(dto);
            Guid taskId = Guid.Empty;
            if (accountId != Guid.Empty)
            {
                var oldEntity = await taskRepository.Get<Guid>(entity.TaskId);
                // 成功标识
                bool flag;
                // 不存在就添加
                if (oldEntity == null)
                {
                    entity.TaskId = Guid.NewGuid();
                    // 记录登录人的id
                    entity.AccountId = accountId;
                    taskId = await taskRepository.Insert<Guid>(entity);
                    flag = taskId != Guid.Empty;
                }
                else
                {
                    // 修改执行时间
                    entity = oldEntity;
                    entity.InvokeDate = DateTime.Now;
                    flag = await taskRepository.Update(entity);
                }
            }

            var result = await func.Invoke(entity);
            return result;
        }
    }
}
