using MaskCrawler.Models.Domain;

using System;

namespace MaskCrawler.Models.Dto
{
    public class TaskInfoDto
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public Guid AccountGid { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public Guid TaskId { get; set; }

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

        /// <summary>
        /// 任务响应类型
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? InvokeDate { get; set; }
    }
}

