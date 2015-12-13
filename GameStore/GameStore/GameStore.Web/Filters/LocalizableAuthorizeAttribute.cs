using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web.Filters
{
    public class LocalizableAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var route = new RouteValueDictionary();
            route["lang"] = filterContext.RouteData.Values["lang"];
            route["action"] = "Login";
            route["controller"] = "Account";
            filterContext.Result = new RedirectToRouteResult(route);
        }
    }
}