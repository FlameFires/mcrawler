using MaskCrawler.Models.Http;

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MaskCrawler.Http
{
    public interface IHttpDecorator
    {
        public Task<byte[]> ReqBytes(IHttpInfo httpInfo = null);

        public Task<MemoryStream> ReqStream(IHttpInfo httpInfo = null);

        public Task<string> ReqString(IHttpInfo httpInfo = null, Encoding encoding = null);
    }
}
