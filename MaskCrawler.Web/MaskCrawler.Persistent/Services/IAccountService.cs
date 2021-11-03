using MaskCrawler.Models.Domain;
using MaskCrawler.Models.Dto;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Services
{
    public interface IAccountService : IBaseService<AccountEntity>
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IActionResult> Login(AccountLoginDto dto, Func<AccountEntity, string> jwtInjectionAction = null);

        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IActionResult> Register(AccountRegisterDto dto);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IActionResult> Query(Expression<Func<AccountEntity, bool>> expression);
    }
}
