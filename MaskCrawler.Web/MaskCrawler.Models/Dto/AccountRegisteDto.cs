namespace MaskCrawler.Models.Dto
{
    public class AccountRegisterDto : AccountLoginDto
    {
        public string Ensure { get; set; }
        public bool IsRemeber { get; set; }
    }
}
