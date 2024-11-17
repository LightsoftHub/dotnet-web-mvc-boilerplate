using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArch.eCode.WebApp.Core.Extensions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class DenyDirectAccessAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var canAcess = false;

        // check the refer
        var referer = filterContext.HttpContext.Request.Headers.Referer.ToString();
        if (!string.IsNullOrEmpty(referer))
        {
            var rUri = new UriBuilder(referer).Uri;
            var req = filterContext.HttpContext.Request;
            if (req.Host.Host == rUri.Host // domain
                && req.Scheme == rUri.Scheme // http, https
                && (req.Host.Port is null || req.Host.Port == rUri.Port)) // fix when deploy to port 80 will not need port to access
            {
                canAcess = true;
            }
        }

        // ... check other requirements
        if (!canAcess)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
        }
    }
}