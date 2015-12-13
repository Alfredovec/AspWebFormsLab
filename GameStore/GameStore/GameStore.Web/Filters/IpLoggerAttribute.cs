using System.Web.Mvc;
using GameStore.Models.Utils;

namespace GameStore.Web.Filters
{
    public class IpLoggerAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public IpLoggerAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logger.LogInfo(string.Format("Access to {0} by {1} ({2})", 
                filterContext.HttpContext.Request.Url.OriginalString,
                filterContext.HttpContext.Request.UserHostAddress,
                filterContext.HttpContext.User.Identity.Name == "" ? "Not Authorized" : filterContext.HttpContext.User.Identity.Name));
            base.OnActionExecuting(filterContext);
        }
    }
}