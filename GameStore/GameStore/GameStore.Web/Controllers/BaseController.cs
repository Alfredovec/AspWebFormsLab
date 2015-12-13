using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.Models.Services;

namespace GameStore.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IStoreServices _storeServices;
        
        public string CurrentLangCode { get; protected set; }

        protected BaseController(IStoreServices storeServices)
        {
            _storeServices = storeServices;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.RouteData.Values["lang"] != null && requestContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLangCode = requestContext.RouteData.Values["lang"] as string;
                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                Thread.CurrentThread.CurrentCulture.NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalSeparator = ".",
                    NumberDecimalSeparator = "."
                };
            }
            base.Initialize(requestContext);
        }
    } 

}
