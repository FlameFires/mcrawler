using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MaskCrawler.Utils
{
    public static class HttpContextUtil
    {
        public static StringValues GetJwtToken(this HttpContext context) => context.GetHeader("Authorization");
        public static StringValues GetHeader(this HttpContext context, string key) => context.Request.Headers.TryGetValue(key, out var value) ? value : StringValues.Empty;
    }
}
