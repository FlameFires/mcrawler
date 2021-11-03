using MaskCrawler.Models.Dto;
using MaskCrawler.Persistent.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MaskCrawler.Controllers.Main
{

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
        /// 暂停任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Pause(Guid id) => await taskService.Pause(id);

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


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskInfoDto taskDto) => await taskService.Update(taskDto);


        /// <summary>
        /// 获取任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Aquire(Guid id) => await taskService.Aquire(id);
    }
}
