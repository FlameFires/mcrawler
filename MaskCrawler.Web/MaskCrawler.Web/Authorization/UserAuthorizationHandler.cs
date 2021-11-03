using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Threading.Tasks;

namespace MaskCrawler.Web.Authorization
{
    /// <summary>
    /// 使用的用户认证方法
    /// </summary>
    internal class UserAuthorizationHandler : AuthorizationHandler<UserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
        {
            var ty = context.Resource.GetType();
            var httpContext = context.Resource as AuthorizationFilterContext; ;


            //string guid = "";
            //if (httpContext.User.Identity.IsAuthenticated)
            //{
            //	var auth = httpContext.AuthenticateAsync().Result.Principal.Claims;
            //	var guidClaim = auth.FirstOrDefault(s => s.Type == "Guid");

            //	if (guidClaim != null)
            //	{
            //		guid = guidClaim.Value;
            //		// 根据Guid获取用户信息（该方法是自己编写的）
            //		//if (GetUserByGuid(guid, out Client user))
            //		//{
            //		//	// 验证成功且拥有权限
            //		//	if (requirement.CheckPermission(user))
            //		//	{
            //		//		context.Succeed(requirement);
            //		//	}
            //		//	else
            //		//	{
            //		//		// 验证成功但权限不足
            //		//		httpContext.Response.Redirect($"api/identify/forbidden");
            //		//	}
            //		//}
            //		//else
            //		//{
            //		//	// 验证成功，但Guid非法
            //		//	httpContext.Response.Redirect($"api/identify/forbidden");
            //		//}
            //	}
            //	else
            //	{
            //		// 验证成功，但没有包含Guid
            //		httpContext.Response.Redirect($"api/identify/forbidden");
            //	}
            //}
            //else
            //{
            //	// 验证失败，没有包含验证信息
            //	httpContext.Response.Redirect($"api/identify/forbidden");
            //}

            return Task.CompletedTask;
        }

    }
}
