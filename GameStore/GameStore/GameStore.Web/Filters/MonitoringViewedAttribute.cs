using System;
using System.Web.Mvc;
using GameStore.Models.Services;
using Ninject;

namespace GameStore.Web.Filters
{
    public class MonitoringViewedAttribute : ActionFilterAttribute
    {
        public string ParamName { get; set; }

        [Inject]
        private readonly IStoreServices _services;

        public MonitoringViewedAttribute()
        {
            _services = DependencyResolver.Current.GetService<IStoreServices>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ParamName != null)
            {
                var param = filterContext.ActionParameters[ParamName];
                try
                {
                    _services.GameService.ViewGame(param.ToString());
                }
                catch (ArgumentException) { }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}