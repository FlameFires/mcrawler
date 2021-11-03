using System;
using System.Collections.Specialized;
using System.Net.Http;

namespace MaskCrawler.Models.Http
{
    public interface IHttpInfo
    {
        string Url { get; set; }
        Uri Uri { get; set; }
        HttpMethod Method { get; set; }
        String Cookie { get; set; }
        string Header { get; set; }
        NameValueCollection HeaderKV { get; set; }
        bool KeepAlive { get; set; }
        bool AllowRedirect { get; set; }
    }
}
