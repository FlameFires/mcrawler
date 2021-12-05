
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MaskCrawler.Http
{
    public abstract class HttpAssembler : IHttpDecorator, IDisposable
    {
        #region fields and properies

        /// <summary>
        /// 请求链接
        /// </summary>
        protected Uri _reqUri;
        /// <summary>
        /// 请求对象
        /// </summary>
        protected HttpWebRequest _req;
        /// <summary>
        /// 响应对象
        /// </summary>
        protected HttpWebResponse _res;
        /// <summary>
        /// 请求字节
        /// </summary>
        protected byte[] _reqData;
        /// <summary>
        /// 请求流
        /// </summary>
        protected MemoryStream _resMS;
        /// <summary>
        /// 请求头部
        /// </summary>
        protected NameValueCollection _header;
        /// <summary>
        /// 请求信息配置
        /// </summary>
        protected IHttpInfo _httpInfo;
        /// <summary>
        /// 响应数据的最大缓存读取字节数
        /// </summary>
        protected int _resReadLen = 1024 * 2;

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }
        public Exception Exception { get; set; }

        public event Func<Exception, IActionResult> ErrorHandler;
        #endregion

        #region construct functions

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
        #endregion

        #region help for requests

        /// <summary>
        /// 验证头部信息
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected void ValidateHttpInfo(IHttpInfo httpInfo)
        {
            if (httpInfo != null)
            {
                _httpInfo = httpInfo;
            }
            if (_httpInfo is null)
            {
                throw new ArgumentNullException(nameof(httpInfo));
            }
        }

        /// <summary>
        /// 解析头部信息
        /// </summary>
        protected virtual void ResolveHeader()
        {
            if (_httpInfo == null || string.IsNullOrEmpty(_httpInfo.Header) && _httpInfo.HeaderKV == null) return;

            var nvc = new NameValueCollection();

            // 将头部信息储存
            if (!string.IsNullOrEmpty(_httpInfo.Header))
            {
                string[] kvs = _httpInfo.Header.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
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
            specialNames.Add("accept");
            specialNames.Add("contenttype");
            specialNames.Add("connection");
            specialNames.Add("cookie");
            foreach (string name in nvc.AllKeys)
            {
                var n = name.Replace("-", "").ToLower();
                if (specialNames.Contains(n))
                {
                    switch (n)
                    {
                        case "useragent": _req.UserAgent = nvc[name]; break;
                        case "accept": _req.Accept = nvc[name]; break;
                        case "contenttype": _req.ContentType = nvc[name]; break;
                        case "connection":
                            if (nvc[name].Equals("keep-alive", StringComparison.InvariantCultureIgnoreCase))
                            {
                                _req.KeepAlive = true;
                            }
                            break;
                        case "cookie": addCookie(nvc[name]); break;
                    };
                    nvc.Remove(name);
                }
            }

            _req?.Headers.Add(nvc);
        }

        protected void addCookie(string cookie)
        {
            if (string.IsNullOrEmpty(cookie)) return;

            var kvs = cookie.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (_req.CookieContainer == null)
                _req.CookieContainer = new CookieContainer();
            foreach (var kv in kvs)
            {
                var vals = kv.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
                if (vals.Length == 2)
                {
                    var ck = new Cookie();

                    ck.Name = vals[0].Trim();
                    ck.Value = encoding(vals[1].Trim());
                    ck.Domain = _req.Host;
                    _req.CookieContainer.Add(ck);
                }
            }
        }

        string encoding(string val)
        {
            return HttpUtility.UrlEncode(val, Encoding.GetEncoding("utf-8"));
        }
        protected static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        protected virtual async Task Init(IHttpInfo httpInfo = null)
        {
            try
            {
                ValidateHttpInfo(httpInfo);
                await InitReq();
                await InitRes();
            }
            catch (Exception ex)
            {
                StatusCode = -1;
                Exception = ex;
                ErrorHandler?.Invoke(ex);
            }
        }

        /// <summary>
        /// 配置请求
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <returns></returns>
        protected virtual async Task InitReq()
        {
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

        /// <summary>
        /// 配置响应数据
        /// </summary>
        /// <returns></returns>
        protected async Task InitRes()
        {
            try
            {
                _res = (HttpWebResponse)await _req.GetResponseAsync();
                StatusCode = (int)_res.StatusCode;
                await resovlerResponseStream();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected async Task resovlerResponseStream()
        {
            _resMS = new MemoryStream();

            if (_res.ContentEncoding == null)
            {
                using (Stream stream = _res.GetResponseStream())
                {
                    byte[] tempBytes = new byte[_resReadLen]; // 缓存容器
                    int totalRead = 0,  // 共读取字节数
                        actuallyRead = 0;  // 实际读取的字节数
                    actuallyRead = stream.Read(tempBytes, 0, _resReadLen);
                    while (actuallyRead > 0)
                    {
                        await _resMS.WriteAsync(tempBytes, 0, actuallyRead);
                        totalRead += actuallyRead;
                        actuallyRead = stream.Read(tempBytes, 0, _resReadLen);
                    }
                    await _resMS.FlushAsync();
                }
            }
            else if (_res.ContentEncoding.ToLower().Contains("gzip"))
            {
                using (GZipStream stream = new GZipStream(_res.GetResponseStream(), CompressionMode.Decompress))
                {
                    _ = stream.CopyToAsync(_resMS);
                }
            }
            else if (_res.ContentEncoding.ToLower().Contains("deflate"))
            {
                using (DeflateStream stream = new DeflateStream(_res.GetResponseStream(), CompressionMode.Decompress))
                {
                    _ = stream.CopyToAsync(_resMS);
                }
            }
            _resMS.Position = 0;
        }

        #endregion
        public void Dispose()
        {
            _req?.Abort();
            _res?.Dispose();
            _resMS?.Dispose();
        }

        public abstract Task<byte[]> ReqBytes(IHttpInfo httpInfo = null);

        public abstract Task<MemoryStream> ReqStream(IHttpInfo httpInfo = null);

        public abstract Task<string> ReqString(IHttpInfo httpInfo = null);

        public abstract Task<Tuple<string, IList<string>>> ReqAndResolve(ResolverInfo resolverInfo, IHttpInfo httpInfo = null);
    }
}
