using MaskCrawler.Models.Http;

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

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <returns></returns>
        public override async Task<byte[]> ReqBytes(IHttpInfo httpInfo = null)
        {
            await Init(httpInfo);
            return _resMS.ToArray();
        }

        public override async Task<MemoryStream> ReqStream(IHttpInfo httpInfo = null)
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
        public override async Task<string> ReqString(IHttpInfo httpInfo = null, Encoding encoding = null)
        {
            await Init(httpInfo);
            if (_resMS == null || _resMS.Length < 1) return null;

            encoding = encoding ?? Encoding.Default;
            StreamReader sr = new StreamReader(_resMS, encoding);
            var str = await sr.ReadToEndAsync();
            sr.Close();
            return str;
        }
    }
}
