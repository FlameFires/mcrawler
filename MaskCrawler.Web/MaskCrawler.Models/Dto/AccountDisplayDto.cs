using System;

namespace MaskCrawler.Models.Dto
{
    public class AccountDisplayDto
    {
        /// <summary>
        /// 账户唯一值
        /// </summary>
        public Guid Gid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}
