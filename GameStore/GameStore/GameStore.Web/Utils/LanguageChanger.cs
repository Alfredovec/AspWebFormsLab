using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web.Utils
{
    public static class LanguageChanger
    {
        public static RouteValueDictionary CreateLanguageRoute(this RouteValueDictionary routes, string lang)
        {
            var res = new RouteValueDictionary();
            foreach (var key in routes.Keys)
            {
                res[key] = routes[key];
            }
            res["lang"] = lang;
            return res;
        }
    }
}