using System;

namespace MaskCrawler.Models.Authroize
{
    public class SessionEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string Gid { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public Int32 Role { get; set; }
    }
}
