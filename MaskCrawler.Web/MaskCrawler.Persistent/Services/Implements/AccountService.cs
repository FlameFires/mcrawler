using MaskCrawler.Models.Domain;
using MaskCrawler.Models.Dto;
using MaskCrawler.Persistent.Repositories;
using MaskCrawler.Utils;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MaskCrawler.Persistent.Services.Implements
{

    public class AccountService : BaseService<AccountEntity>, IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IJwtService jwtService;

        public AccountService(IAccountRepository accountRepository, IJwtService jwtService) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this.jwtService = jwtService;
        }

        public async Task<IActionResult> Login(AccountLoginDto dto, Func<AccountEntity, string> jwtInjectionAction = null)
        {
            // 查询账户是否存在
            var entities = (await accountRepository.GetList(x => x.Email == dto.Name || x.Phone == dto.Name))?.ToList();
            if (entities == null || entities.Count == 0) return BackResult.Failed("用户不存在");

            var salt = entities[0].Salt;
            var pwd = SecurityUtil.Md5Encrypt(dto.Pwd + salt);

            // 支持邮箱与手机号
            entities = (await accountRepository.GetList(x => (x.Email == dto.Name || x.Phone == dto.Name) && x.Pwd == pwd))?.ToList();

            if (entities == null || entities.Count == 0) return BackResult.Failed("用户不存在");

            var jwtToken = jwtInjectionAction?.Invoke(entities[0]);
            // TODO set jwttoken on header

            return BackResult.Judge(entities != null, "登录成功", data: jwtToken);
        }

        public async Task<IActionResult> Query(Expression<Func<AccountEntity, bool>> expression)
        {
            var list = (await GetList(expression))?.ToList();
            if (list != null && list.Count > 0)
            {
                var account = list[0];

                AccountDisplayDto dto = new AccountDisplayDto
                {
                    Gid = account.Gid,
                    Name = account.Name,
                    Email = account.Email,
                    Phone = account.Phone
                };

                return BackResult.Successed(data: dto);
            }

            return BackResult.Successed("没有此用户数据");
        }

        public async Task<IActionResult> Register(AccountRegisterDto dto)
        {
            if (!dto.Pwd.Equals(dto.Ensure))
            {
                return BackResult.Successed("密码不一致");
            }

            var entities = (await accountRepository.GetList(x => x.Email == dto.Name || x.Phone == dto.Name))?.ToList();
            if (entities != null && entities.Count > 0) return BackResult.Successed("账户已存在");

            var salt = StringUtil.RandomGetStr(6);
            var tempPwd = dto.Pwd + salt;

            AccountEntity entity = new AccountEntity
            {
                Name = dto.Name,
                Pwd = tempPwd.Md5Encrypt(),
                Salt = salt,
            };

            var num = await accountRepository.Insert(entity);

            // TODO 转换数据

            return BackResult.Judge(num, "注册成功");
        }
    }
}
