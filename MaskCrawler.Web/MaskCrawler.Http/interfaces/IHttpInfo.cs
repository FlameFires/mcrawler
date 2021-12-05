
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;

namespace MaskCrawler.Http
{
    public interface IHttpInfo
    {
        string Url { get; set; }
        Uri Uri { get; set; }
        HttpMethod Method { get; set; }
        string Cookie { get; set; }
        string Header { get; set; }
        NameValueCollection HeaderKV { get; set; }
        bool KeepAlive { get; set; }
        bool AllowRedirect { get; set; }

        /// <summary>
        /// 返回内容解码
        /// </summary>
        Encoding ContentEncoding { get; set; }

    }
}
