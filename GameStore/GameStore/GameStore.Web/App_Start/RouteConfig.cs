using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "PublisherRouteCreateLang",
                url: "{lang}/publisher/{action}",
                defaults: new {controller = "Publisher", lang = "ru"},
                constraints: new { lang = @"ru|en", action = @"new|all|edit|delete" }
                );

            routes.MapRoute(
                name: "PublisherRouteDetailsLang",
                url: "{lang}/publisher/{*companyName}",
                defaults: new { controller = "Publisher", action = "Details", lang="ru" },
                constraints: new { lang = @"ru|en" }
                );

            routes.MapRoute(
                name: "GameRouteBuyLang",
                url: "{lang}/game/{gameKey}/buy",
                defaults: new { controller = "Basket", action = "Add", lang = "ru" },
                constraints: new { lang = @"ru|en" }
                );

            routes.MapRoute(
                name: "GameRouteDownloadLang",
                url: "{lang}/game/{gameKey}/download",
                defaults: new { controller = "Game", action = "Download", lang = "ru" },
                constraints: new { lang = @"ru|en" }
                );

            routes.MapRoute(
                name: "CommentrouteLang",
                url: "{lang}/game/{gameKey}/{action}",
                defaults: new { controller = "Comment", lang = "ru" },
                constraints: new { lang = @"ru|en" }
                );

            routes.MapRoute(
                name: "GameRouteExtendedLang",
                url: "{lang}/game/{gameKey}",
                defaults: new { controller = "Game", action = "Get", lang = "ru" },
                constraints: new { lang = @"ru|en" }
                );

            routes.MapRoute(
                name: "GameRouteLang",
                url: "{lang}/games/{action}/{gameKey}",
                defaults: new { controller = "Game", action = "Index", gameKey = UrlParameter.Optional, lang="ru" },
                constraints: new { lang = @"ru|en" }
                );

            routes.MapRoute(
                name: "ORderRouteLang",
                url: "{lang}/order",
                defaults: new { controller = "Basket", action = "PreparePayment", lang="ru"},
                constraints: new { lang = @"ru|en" }
                );

            routes.MapRoute(
                name: "DefaultLang",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new {controller = "Game", action = "Index", id = UrlParameter.Optional, lang = "ru"},
                constraints: new {lang = @"ru|en"}
                );

            routes.MapRoute(
                name: "GameLogoAsyncRoute",
                url: "gamelogoAsync/{id}",
                defaults: new { controller = "Game", lang = "ru", action = "GameImageAsync" }
                );

            routes.MapRoute(
                name: "GameLogoRoute",
                url: "gamelogo/{id}",
                defaults: new { controller = "Game", lang = "ru", action = "GameImage" }
                );

            routes.MapRoute(
                name: "PublisherRouteCreate",
                url: "publisher/{action}",
                defaults: new {controller = "Publisher", lang = "ru"},
                constraints: new {action = @"new|all|edit|delete"}
                );

            routes.MapRoute(
                name: "PublisherRouteDetails",
                url: "publisher/{*companyName}",
                defaults: new { controller = "Publisher", action = "Details", lang="ru" }
                );

            routes.MapRoute(
                name: "GameRouteBuy",
                url: "game/{gameKey}/buy",
                defaults: new { controller = "Basket", action = "Add", lang = "ru" }
                );

            routes.MapRoute(
                name: "GameRouteDownload",
                url: "game/{gameKey}/download",
                defaults: new { controller = "Game", action = "Download", lang = "ru" }
                );

            routes.MapRoute(
                name: "Commentroute",
                url: "game/{gameKey}/{action}",
                defaults: new { controller = "Comment", lang = "ru" }
                );

            routes.MapRoute(
                name: "GameRouteExtended",
                url: "game/{gameKey}",
                defaults: new { controller = "Game", action = "Get", lang = "ru" }
                );

            routes.MapRoute(
                name: "GameRoute",
                url: "games/{action}/{gameKey}",
                defaults: new { controller = "Game", action = "Index", gameKey = UrlParameter.Optional, lang="ru" }
                );

            routes.MapRoute(
                name: "ORderRoute",
                url: "order",
                defaults: new { controller = "Basket", action = "PreparePayment", lang="ru"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional, lang = "ru" }
            );
        }
    }
}