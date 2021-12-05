using FluentValidation;

using MaskCrawler.Models.Dto;

namespace MaskCrawler.Models
{
    public class TaskInfoDtoValidator : AbstractValidator<TaskInfoDto>
    {
        public TaskInfoDtoValidator()
        {
            RuleFor(x => x.Url).Matches("(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]")
                               .NotEmpty()
                               .WithMessage("链接不合法");
            RuleFor(x => x.TaskName)
                               .NotEmpty()
                               .WithMessage("任务名不能为空");
            // Get,post,put,delete 验证
            RuleFor(x => x.Method)
                               .NotEmpty()
                               .WithMessage("请求类型不合法");
        }
    }
}
