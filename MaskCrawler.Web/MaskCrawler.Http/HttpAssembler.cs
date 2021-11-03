using MaskCrawler.Models.Http;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MaskCrawler.Http
{
    public abstract class HttpAssembler : IHttpDecorator, IDisposable
    {
        protected Uri _reqUri;
        protected HttpWebRequest _req;
        protected HttpWebResponse _res;
        protected Stream resStream;
        protected byte[] _reqData;
        protected MemoryStream _resMS;
        protected NameValueCollection _header;
        protected IHttpInfo _httpInfo;
        protected int _resReadLen = 1024 * 2;

        public HttpAssembler()
        {
        }
        public HttpAssembler(IHttpInfo httpInfo)
        {
            _httpInfo = httpInfo;
        }

        static HttpAssembler()
        {
            ServicePointManager.DefaultConnectionLimit = 1024;
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
        }

        protected virtual void ResolveHeader()
        {
            if (_httpInfo == null || (string.IsNullOrEmpty(_httpInfo.Header) && _httpInfo.HeaderKV == null)) return;

            var nvc = new NameValueCollection();

            if (!string.IsNullOrEmpty(_httpInfo.Header))
            {
                string[] kvs = _httpInfo.Header.Split(new string[] { "\n" }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (kvs.Length != 2) return;
                foreach (var item in kvs)
                {
                    string[] kv = item.Split(new string[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (kv == null || kv.Length != 2) continue;
                    var name = kv[0];
                    var value = kv[1];
                    nvc.Add(name, value);
                }
            }

            if (_httpInfo.HeaderKV != null && _httpInfo.HeaderKV.Count > 0)
            {
                nvc.Add(_httpInfo.HeaderKV);
            }


            // 过滤
            var specialNames = new List<string>();
            specialNames.Add("useragent");
            foreach (string name in nvc.AllKeys)
            {
                var n = name.Replace("-", "").ToLower();
                if (specialNames.Contains(n))
                {
                    nvc.Remove(name);
                    switch (n)
                    {
                        case "useragent": _req.UserAgent = nvc[name]; break;
                    };

                }
            }

            _req?.Headers.Add(nvc);
        }

        protected static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        protected virtual async Task Init(IHttpInfo httpInfo = null)
        {
            await InitReq(httpInfo);
            await InitRes();
        }

        protected virtual async Task InitReq(IHttpInfo httpInfo = null)
        {
            if (httpInfo != null) _httpInfo = httpInfo;

            _reqUri = new Uri(_httpInfo.Url);
            try
            {
                _req = WebRequest.CreateDefault(_reqUri) as HttpWebRequest;
                _req.Method = _httpInfo.Method.ToString();
                ResolveHeader();

                var method = _httpInfo.Method.ToString();
                if (method == "post" || method == "put")
                    using (var reqStream = await _req.GetRequestStreamAsync())
                    {
                        _ = reqStream.WriteAsync(_reqData);
                        _ = reqStream.FlushAsync();
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async Task InitRes()
        {
            if (_req == null) return;

            try
            {
                _res = (await _req.GetResponseAsync()) as HttpWebResponse;

                resStream = _res.GetResponseStream();
                byte[] tempBytes = new byte[_resReadLen]; // 缓存容器
                int totalRead = 0,  // 共读取字节数
                    actuallyRead = 0;  // 实际读取的字节数
                _resMS = new MemoryStream();
                actuallyRead = resStream.Read(tempBytes, 0, _resReadLen);
                while (actuallyRead > 0)
                {
                    await _resMS.WriteAsync(tempBytes, 0, actuallyRead);
                    totalRead += actuallyRead;
                    actuallyRead = resStream.Read(tempBytes, 0, _resReadLen);
                }
                await _resMS.FlushAsync();
                _resMS.Position = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            _req.Abort();
            _res.Dispose();
            _resMS?.Dispose();
        }

        public abstract Task<byte[]> ReqBytes(IHttpInfo httpInfo = null);

        public abstract Task<MemoryStream> ReqStream(IHttpInfo httpInfo = null);

        public abstract Task<string> ReqString(IHttpInfo httpInfo = null, Encoding encoding = null);
    }
}
