using MaskCrawler.Models.Authroize;
using MaskCrawler.Models.Domain;
using MaskCrawler.Models.Dto;
using MaskCrawler.Persistent.Services;
using MaskCrawler.Utils;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaskCrawler.Controllers
{
    /// <summary>
    /// 账号信息接口
    /// </summary>
    public class AccountController : MainController
    {
        private readonly IAccountService accountService;
        private readonly IJwtService jWTService;

        public AccountController(IAccountService accountService, IJwtService jWTService)
        {
            this.accountService = accountService;
            this.jWTService = jWTService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        public async Task<IActionResult> Login(AccountLoginDto dto)
        {
            var aciton = new Func<AccountEntity, string>(entity =>
            {
                var gid = entity.Gid;
                var token = jWTService.GetToken(new SessionEntity
                {
                    Gid = entity.Gid.ToString(),
                    Name = entity.Name,
                    Role = 1
                });
                base.HttpContext.Response.Headers.Add("token", token);
                return token;
            });
            return await accountService.Login(dto, aciton);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterDto dto)
        {
            return await accountService.Register(dto);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Aquire()
        {
            var jwtToken = base.HttpContext.GetJwtToken();
            if (jwtToken == Microsoft.Extensions.Primitives.StringValues.Empty || jwtToken.Count < 1)
                return BackResult.Failed("没获取到用户信息");

            List<System.Security.Claims.Claim> claims = jWTService.GetClaims(base.HttpContext).ToList();
            var gid = jWTService.GetClaimValue(base.HttpContext, nameof(SessionEntity.Gid))?.Value;

            return await accountService.Query(x => x.Gid == new Guid(gid));
        }


        /// <summary>
        /// 校验token并返回
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult CheckAuthorize()
        {
            try
            {
                //获取claims
                var claims = base.HttpContext.AuthenticateAsync().Result.Principal.Claims.ToList();
                //获取请求的token
                var token = base.HttpContext.AuthenticateAsync().Result.Properties.Items.ToArray()[0].Value;
                string json = jWTService.DecodeToken(base.HttpContext);
                return new JsonResult(new
                {
                    Data = "已授权",
                    Type = "CheckAuthorize",
                    Claim = claims[0].Issuer,
                    Json = json
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    Data = "未授权",
                    Type = "CheckAuthorize",
                    Exception = ex.ToString()
                });
            }
        }
    }
}
