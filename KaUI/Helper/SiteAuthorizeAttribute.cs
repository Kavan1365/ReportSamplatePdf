using KaUI.Configuration.Helper;
using KaUI.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace KaUI.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class SiteAuthorizeAttribute : AuthorizeAttribute
    {
       
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                var _httpService = context.HttpContext.RequestServices.GetService<IHttpService>();
                var _urlHelper = context.HttpContext.RequestServices.GetService<UrlHelper>();

                var GetAuth = await _httpService.Get(_urlHelper.UrlBase + "Identity");
                if (!GetAuth.isSuccess)
                {
                    await context.HttpContext.SignOutAsync();
                }
                return;
            }
          
        }
    }
}
