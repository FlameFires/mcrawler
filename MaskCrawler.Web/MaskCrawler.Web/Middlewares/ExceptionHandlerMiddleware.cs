using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace MaskCrawler.Middlewares
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public ExceptionHandlerMiddleware(ILogger logger)
        {
            _logger = logger;
        }

        private Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(msg);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                //这里也可以进行请求和响应日志的的记录
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                //进行异常处理
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = String.Empty;
                switch (statusCode)
                {
                    case 500:
                        msg = "服务器系统内部错误";
                        break;

                    case 401:
                        msg = "未登录";
                        break;

                    case 403:
                        msg = "无权限执行此操作";
                        break;

                    case 408:
                        msg = "请求超时";
                        break;
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }
    }

}
