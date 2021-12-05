using MaskCrawler.Middlewares;

using Microsoft.AspNetCore.Builder;

namespace MaskCrawler.Extensions
{
    public static class ExceptionHandlerExtension
    {
        /// <summary>
        /// 使用自定义异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSelfExceptionHandler(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
