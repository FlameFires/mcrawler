using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaskCrawler.Models.Domain
{
    [Table("task")]
    public class TaskEntity
    {
        public TaskEntity()
        {
        }

        public TaskEntity Init()
        {
            if(TaskId == default) TaskId = Guid.NewGuid();
            CreateDate = DateTime.Now;
            return this;
        }

        /// <summary>
        /// 任务id
        /// </summary>
        [Column("tid")]
        [Key]
        public Guid TaskId { get; set; }
        /// <summary>
        /// 任务父id
        /// </summary>
        [Column("tpid")]
        public Guid TaskParentId { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        [Column("name")]
        public string TaskName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [Column("describe")]
        public string TaskDescribe { get; set; }

        #region 10.21 新增

        /// <summary>
        /// 账户id
        /// </summary>
        [Column("accountId")]
        public Guid AccountId { get; set; }

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
        [Column("rType")]
        public string ResolveType { get; set; }

        /// <summary>
        /// 解析表达式
        /// </summary>
        [Column("rPattern")]
        public string ResolvePattern { get; set; }

        #endregion

        /// <summary>
        /// 任务响应类型
        /// </summary>
        [Column("resType")]
        public string ResponseType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
    }






}
