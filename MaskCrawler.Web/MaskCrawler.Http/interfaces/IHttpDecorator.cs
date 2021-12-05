
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MaskCrawler.Http
{
    public interface IHttpDecorator
    {
        event Func<Exception, IActionResult> ErrorHandler;

        /// <summary>
        /// 异常信息
        /// </summary>
        Exception Exception { get; set; }
        /// <summary>
        /// 状态码 -1: 出现异常 
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// 发送请求并获取返回内容字节
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <returns></returns>
        public Task<byte[]> ReqBytes(IHttpInfo httpInfo = null);

        /// <summary>
        /// 发送请求并获取返回内存流
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <returns></returns>
        public Task<MemoryStream> ReqStream(IHttpInfo httpInfo = null);

        /// <summary>
        /// 发送请求并获取返回内容
        /// </summary>
        /// <param name="httpInfo"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public Task<string> ReqString(IHttpInfo httpInfo = null);


        /// <summary>
        /// 发送请求并获取解析内容
        /// </summary>
        /// <param name="resolverInfo"></param>
        /// <param name="httpInfo"></param>
        /// <returns></returns>
        public abstract Task<Tuple<string, IList<string>>> ReqAndResolve(ResolverInfo resolverInfo, IHttpInfo httpInfo = null);

    }
}
