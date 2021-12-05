namespace MaskCrawler.Models.Dto
{
    public class AccountRegisterDto : AccountLoginDto
    {
        /// <summary>
        /// 确认密码
        /// </summary>
        public string Ensure { get; set; }
        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool IsRemeber { get; set; }
    }
}
