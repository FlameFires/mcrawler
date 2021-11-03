using FluentValidation;

using MaskCrawler.Models.Dto;

namespace MaskCrawler.Models
{
    public class TaskInfoDtoValidator : AbstractValidator<TaskInfoDto>
    {
        public TaskInfoDtoValidator()
        {
            RuleFor(x => x.Url).Matches("(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]").NotEmpty().WithMessage("账户名不能为空");
        }
    }
}
