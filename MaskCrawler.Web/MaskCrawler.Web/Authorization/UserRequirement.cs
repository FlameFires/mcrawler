using Microsoft.AspNetCore.Authorization;

namespace MaskCrawler.Web.Authorization
{
    /// <summary>
    /// 用户认证需求
    /// </summary>
    internal class UserRequirement : IAuthorizationRequirement
    {
        public string UserIdentity { get; private set; }

        public UserRequirement(string UserIdentityName)
        {
            UserIdentity = UserIdentityName;
        }

    }
}
