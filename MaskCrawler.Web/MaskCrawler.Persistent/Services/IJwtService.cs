using MaskCrawler.Models.Authroize;

using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Security.Claims;

namespace MaskCrawler.Persistent.Services
{
    public interface IJwtService
    {
        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        string GetToken(SessionEntity entity);

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        string DecodeToken(HttpContext context);


        /// <summary>
        /// 获取负荷
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        IEnumerable<Claim> GetClaims(HttpContext context);


        /// <summary>
        /// 获取某个负荷的值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Claim GetClaimValue(HttpContext context, string type);

    }
}
