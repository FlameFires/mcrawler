using FluentValidation;

using MaskCrawler.Models.Dto;

namespace MaskCrawler.Models
{
    public class AccountLoginDtoValidator : AbstractValidator<AccountLoginDto>
    {
        public AccountLoginDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("账户名不能为空");
            RuleFor(x => x.Pwd).NotEmpty().WithMessage("密码不能为空");
        }
    }
}
