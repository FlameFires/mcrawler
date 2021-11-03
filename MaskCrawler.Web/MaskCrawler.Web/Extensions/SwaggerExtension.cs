using FluentValidation.AspNetCore;

using MaskCrawler.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;

using System;

namespace MaskCrawler.Web.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "阴影爬虫",
                    Version = "v1",
                    Description = "阴影爬虫, 一个简洁但不简单的在线爬虫工具。",
                    TermsOfService = new Uri("http://termsofservice.com"),
                    Contact = new OpenApiContact() { Name = "阴影", Email = "fire2019@qq.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("http://test.html.com") },
                });

                #region Jwt
                //开启权限小锁
                option.OperationFilter<AddResponseHeadersFilter>();
                option.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //在header中添加token，传递到后台
                option.OperationFilter<SecurityRequirementsOperationFilter>();
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传递)直接在下面框中输入Bearer {token}(注意两者之间是一个空格) \"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });


                #endregion

                option.IncludeXmlComments("MaskCrawler.xml", true);
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(fu =>
                {
                    fu.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fu.RegisterValidatorsFromAssemblyContaining<AccountLoginDtoValidator>();
                    fu.RegisterValidatorsFromAssemblyContaining<AccountRegisterDtoValidator>();
                    fu.RegisterValidatorsFromAssemblyContaining<TaskInfoDtoValidator>();
                });
            return services;
        }

        public static void UseSwaggerConfigure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
