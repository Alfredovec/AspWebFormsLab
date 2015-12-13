using System.Web.Mvc;
using GameStore.Models.Utils;
using GameStore.Web.Filters;

namespace GameStore.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            ILogger logger = DependencyResolver.Current.GetService<ILogger>();

            filters.Add(new ErrorHadlerFilterAttribute(logger));
            filters.Add(new IpLoggerAttribute(logger));
        }
    }
}