namespace MaskCrawler.Models
{
    public class JwtSetting
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecurityKey { get; set; }
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; }

        public int ExpireMinutes { get; set; }

        public string Credentials { get; set; }
    }
}
