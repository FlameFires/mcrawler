using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaskCrawler.Models.Domain
{
    [Table("account")]
    public class AccountEntity : BaseEntity
    {
        public AccountEntity()
        {
            Gid = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 账户唯一值
        /// </summary>
        [Key]
        public Guid Gid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column("password")]
        public string Pwd { get; set; }
        /// <summary>
        /// 盐值
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
