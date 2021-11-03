using MaskCrawler.Http;
using MaskCrawler.Models;
using MaskCrawler.Persistent.Infrastructure.MySql;
using MaskCrawler.Persistent.Repositories;
using MaskCrawler.Persistent.Repositories.Implements;
using MaskCrawler.Persistent.Services;
using MaskCrawler.Persistent.Services.Implements;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;

namespace MaskCrawler.Web.Extensions
{
    public static class StartupOverallExtension
    {
        /// <summary>
        /// 添加应用服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddOverallServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSwaggerServices();

            services.AddCors(options => options.AddPolicy("AllowCors", builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            // 使用 newtonsoftjson 解析
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //修改属性名称的序列化方式，首字母小写，即驼峰样式
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //解决命名不一致问题 
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                //日期类型默认格式化处理 方式1
                //options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                //日期类型默认格式化处理 方式2
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                options.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";

                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                //空值处理
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            // 添加仓库
            services.AddRepositories(configuration);
            // 添加应用服务
            services.AddServices();
            // 添加鉴权
            services.AddJWTServices(configuration);
        }

        /// <summary>
        /// 启用应用配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public static void UseOverallConfigure(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseRouting(); //启用路由
            app.UseCors("AllowCors");
            app.UseAuthentication();
            app.UseAuthorization(); //使用Authorization
            app.UseStaticFiles(); //启用静态文件
            app.UseSwaggerConfigure(); //启用api配置
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapRazorPages(); //终结点
            });
        }

        /// <summary>
        /// 添加数据仓库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // 数据库配置
            var dbConfig = new DbConfig();
            var dbSection = configuration.GetSection("database");
            dbSection?.Bind(dbConfig);
            // 数据链接配置
            Persistent.Infrastructure.IDatabaseAdapter dataAdapter = dbConfig.Type switch
            {
                Models.DbType.Mysql => new MySqlDataCentre(dbConfig),
                _ => throw new ArgumentException(nameof(dataAdapter))
            };
            dataAdapter.InitDataBase();
            services.AddSingleton(dataAdapter); // 持久
            // 数据仓库
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<ITaskRepository, TaskRepository>();
        }

        /// <summary>
        /// 添加数据服务
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<ITaskService, TaskService>();
            services.AddTransient<IHttpDecorator, HttpDecorator>();
        }
    }
}
