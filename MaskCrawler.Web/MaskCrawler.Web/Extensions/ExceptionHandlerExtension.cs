using MaskCrawler.Middlewares;

using Microsoft.AspNetCore.Builder;

namespace MaskCrawler.Extensions
{
    public static class ExceptionHandlerExtension
    {
        public static IApplicationBuilder UseSelfExceptionHandler(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
