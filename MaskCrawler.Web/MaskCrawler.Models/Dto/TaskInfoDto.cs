using MaskCrawler.Models.Domain;

using System;

namespace MaskCrawler.Models.Dto
{
    public class TaskInfoDto
    {
        public string TaskName { get; set; }
        public string Desc { get; set; }

        public Guid TaskId { get; set; }
        public Guid AccountGid { get; set; }

        /// <summary>
        /// 请求链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 头部信息
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 解析类型
        /// </summary>
        public string ResolveType { get; set; }

        /// <summary>
        /// 解析表达式
        /// </summary>
        public string ResolvePattern { get; set; }

    }
}
