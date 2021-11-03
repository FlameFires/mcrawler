using MaskCrawler.Models;
using MaskCrawler.Models.Authroize;
using MaskCrawler.Utils;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MaskCrawler.Persistent.Services.Implements
{
    /// <summary>
    /// 生成JWT的service类
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtSetting jwtSetting;
        /// <summary>
        /// 在构造函数中注入configuration以拿取appsettings.json中的内容
        /// </summary>
        /// <param name="configuration"></param>
        public JwtService(JwtSetting jwtSetting)
        {
            this.jwtSetting = jwtSetting;
        }


        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public string GetToken(SessionEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            //注：下面调用方法都是使用了默认的header
            //初始化payload
            //创建用户身份标识，可按需要添加更多信息
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(nameof(entity.Gid), entity.Gid.ToString(), ClaimValueTypes.String), // 用户id
                new Claim(nameof(entity.Name), entity.Name), // 用户名
                new Claim(nameof(entity.Role), "1",ClaimValueTypes.Integer32) // 是否是管理员
            };
            //生成对称秘钥
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey));
            //初始化签名凭证
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            /**
             *  Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:
                iss: jwt签发者
                sub: jwt所面向的用户
                aud: 接收jwt的一方
                exp: jwt的过期时间，这个过期时间必须要大于签发时间
                nbf: 定义在什么时间之前，该jwt都是不可用的.
                iat: jwt的签发时间
                jti: jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击。
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            //创建令牌
            var token = new JwtSecurityToken(
              issuer: jwtSetting.Issuer,//设置签发者
              audience: jwtSetting.Audience, //设置接收者
              signingCredentials: creds,//初始化安全令牌参数
              claims: claims, //设置payload
                              //notBefore: DateTime.Now,
              expires: DateTime.Now.AddMinutes(jwtSetting.ExpireMinutes)//5分钟有效期
            );
            //输出token
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }

        public IEnumerable<Claim> GetClaims(HttpContext context)
        {
            var jwtToken = context.GetJwtToken().ToString();
            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                return null;
            }

            if (jwtToken.Contains("Bearer")) jwtToken = jwtToken.Replace("Bearer", "").Trim();

            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
            IEnumerable<Claim> claims = token.Claims;
            return claims;

        }

        public string DecodeToken(HttpContext context)
        {
            var jwtToken = context.GetJwtToken().ToString();
            if (string.IsNullOrWhiteSpace(jwtToken))
            {
                throw new ArgumentException($"“{nameof(jwtToken)}”不能为 null 或空白。", nameof(jwtToken));
            }

            if (jwtToken.Contains("Bearer")) jwtToken = jwtToken.Replace("Bearer", "").Trim();

            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
            return token.ToString();
        }

        public Claim GetClaimValue(HttpContext context, string type)
        {
            var jwtToken = context.GetJwtToken();
            var claims = GetClaims(context);
            var claim = claims.FirstOrDefault(t => t.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            return claim;
        }
    }
}
