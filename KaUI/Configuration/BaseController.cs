using KaUI.Configuration.Constants;
using KaUI.Configuration.Helper;
using KaUI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net.Http;

namespace KaUI.Configuration
{
    [Authorize()]
    public class BaseController : Controller
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IHttpService _httpService;
        public readonly UrlHelper _urlHelper;
        public BaseController(IHttpService httpService, UrlHelper urlHelper, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = accessor;
            _httpService = httpService;
            _urlHelper = urlHelper;
        }
        public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
