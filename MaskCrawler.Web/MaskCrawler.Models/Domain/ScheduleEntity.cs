using System;

namespace MaskCrawler.Models.Domain
{
    /// <summary>
    /// 调度明细表
    /// </summary>
    public class ScheduleEntity
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 账户id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 执行信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
