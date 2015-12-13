using System;
using System.Web.Mvc;
using GameStore.Models.Utils;

namespace GameStore.Web.Filters
{
    public class ErrorHadlerFilterAttribute : HandleErrorAttribute
    {
        private readonly ILogger _log;

        public ErrorHadlerFilterAttribute(ILogger logger)
        {
            _log = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            _log.LogFatal(exception);
            base.OnException(filterContext);
        }
    }
}