using MaskCrawler.Models;
using MaskCrawler.Persistent.Services;
using MaskCrawler.Persistent.Services.Implements;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace MaskCrawler.Web.Extensions
{
    public static class JWTExtension
    {
        public static void AddJWTServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region JWT鉴权注入
            var jwtSetting = new JwtSetting();
            configuration.GetSection("JwtSetting")?.Bind(jwtSetting);
            services.AddSingleton(jwtSetting);
            services.AddSingleton<IJwtService, JwtService>();
            //services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>();
            //将JWTService注入，那样就可以在构造函数中进行注入。如果没加进来，在构造函数中注入configuration是无用的
            //1.Nuget引入程序包：Microsoft.AspNetCore.Authentication.JwtBearer 

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })  //默认授权机制名称   
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,//是否在令牌期间验证签发者
                         ValidateAudience = true,//是否验证接收者
                         ValidateLifetime = true,//是否验证失效时间
                         ValidateIssuerSigningKey = true,//是否验证签名
                         ValidAudience = jwtSetting.Audience,//接收者
                         ValidIssuer = jwtSetting.Issuer,//签发者，签发的Token的人
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey))//拿到SHA256加密后的key
                     };
                 });

            #endregion
        }
    }
}
