using MaskCrawler.Http;
using MaskCrawler.Models.Domain;
using MaskCrawler.Models.Dto;
using MaskCrawler.Persistent.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MaskCrawler.Controllers.Main
{
    [Authorize]
    public class TaskController : MainController
    {
        private readonly ITaskService taskService;
        private readonly IAccountService accountService;
        private readonly IJwtService jwtService;

        public TaskController(ITaskService taskService, IAccountService accountService, IJwtService jwtService)
        {
            this.taskService = taskService;
            this.accountService = accountService;
            this.jwtService = jwtService;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(TaskInfoDto dto)
        {
            var claims = jwtService.GetClaims(base.HttpContext)?.ToList();
            if (claims != null)
            {
                var claim = claims.FirstOrDefault(t => t.Type.Equals("gid", StringComparison.OrdinalIgnoreCase));
                if (claim != null)
                {
                    var gid = new Guid(claim.Value);
                    dto.AccountGid = gid;
                }
            }

            return await taskService.Add(dto);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Start(Guid id) => await taskService.Start(id);

        /// <summary>
        /// 解析请求
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Resolve(TaskEntity entity)
        {
            var result = await taskService.Resolve(entity, base.Request.HttpContext, async entity =>
            {
                var hinfo = new HttpInfo
                {
                    Url = entity.Url,
                    Header = entity.Header,
                    Method = new System.Net.Http.HttpMethod(entity.Method.ToString())
                };

                var rinfo = new ResolverInfo
                {
                    Pattern = entity.ResolvePattern,
                    Type = (ResolverTypeEnum)Enum.Parse(typeof(ResolverTypeEnum), entity.ResolveType),
                };

                HttpDecorator decorator = new HttpDecorator(hinfo);
                var (html, list) = await decorator.ReqAndResolve(rinfo, hinfo);
                return BackResult.Successed(data: new { html = html, result = list });
            });
            return result;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id) => await taskService.Delete(id);

        /// <summary>
        /// 查询任务
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Query([FromQuery] TaskQueryDto queryDto) => await taskService.Query(queryDto);

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskEntity entity) => await taskService.Update(entity);

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Aquire(Guid id) => await taskService.Aquire(id);
    }
}
