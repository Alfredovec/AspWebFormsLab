using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using GameStore.Models.Services;

namespace GameStore.Web.Controllers.api
{
    public class BaseApiController : ApiController
    {
        protected readonly IStoreServices _storeServices;

        public string CurrentLangCode { get; protected set; }

        protected BaseApiController(IStoreServices storeServices)
        {
            _storeServices = storeServices;
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            if (controllerContext.RouteData.Values["lang"] != null && controllerContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLangCode = controllerContext.RouteData.Values["lang"] as string;
                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                Thread.CurrentThread.CurrentCulture.NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalSeparator = ".",
                    NumberDecimalSeparator = "."
                };
            }
            base.Initialize(controllerContext);
        }
    }
}
