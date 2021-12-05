
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MaskCrawler.Http
{
    public class HttpDecorator : HttpAssembler
    {
        public HttpDecorator() : base()
        {
        }

        public HttpDecorator(IHttpInfo httpInfo) : base(httpInfo)
        {
        }

        IResolver _resolver = new SimpleResolver();

        /// <summary>
        /// 请求并解析
        /// </summary>
        /// <param name="resolverInfo"></param>
        /// <param name="httpInfo"></param>
        /// <returns></returns>
        public override async Task<Tuple<string, IList<string>>> ReqAndResolve(ResolverInfo resolverInfo, IHttpInfo httpInfo = null)
        {
            var htmlStr = await ReqString(httpInfo);
            var type = resolverInfo.Type;
            var result = _resolver.Resolve(resolverInfo, htmlStr);
            return Tuple.Create(htmlStr, result);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <returns></returns>
        public override async Task<byte[]> ReqBytes(IHttpInfo httpInfo)
        {
            await Init();
            return _resMS.ToArray();
        }

        public override async Task<MemoryStream> ReqStream(IHttpInfo httpInfo)
        {
            await Init(httpInfo);
            return _resMS;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public override async Task<string> ReqString(IHttpInfo httpInfo)
        {
            await Init(httpInfo);
            if (_resMS == null || _resMS.Length < 1) return null;

            var encoding = _httpInfo.ContentEncoding ??= Encoding.Default;
            StreamReader sr = new StreamReader(_resMS, encoding);
            var str = await sr.ReadToEndAsync();
            sr.Close();
            return str;
        }
    }
}
