
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;

namespace MaskCrawler.Http
{
    public class HttpInfo : IHttpInfo
    {
        public HttpInfo()
        {
            KeepAlive = false;
            AllowRedirect = true;
            Method = new HttpMethod("get");
        }

        public HttpInfo(string url, Uri uri, HttpMethod method, string cookie, string header, NameValueCollection headerKV, bool keepAlive, bool allowRedirect) : this()
        {
            Url = url;
            Uri = uri;
            Method = method;
            Cookie = cookie;
            Header = header;
            HeaderKV = headerKV;
            KeepAlive = keepAlive;
            AllowRedirect = allowRedirect;
        }

        public string Url { get; set; }
        public Uri Uri { get; set; }
        public HttpMethod Method { get; set; }
        public string Cookie { get; set; }
        public string Header { get; set; }
        public NameValueCollection HeaderKV { get; set; }
        public bool KeepAlive { get; set; }
        public bool AllowRedirect { get; set; }
        public Encoding ContentEncoding { get; set; }
    }
}
