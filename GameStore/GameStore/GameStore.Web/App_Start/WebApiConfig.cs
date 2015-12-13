using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Validation.Providers;

namespace GameStore.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "GameGenreApiLang",
                routeTemplate: "api/{lang}/games/{gameId}/genres",
                defaults: new { id = RouteParameter.Optional, lang = "ru", controller = "Games" }
            );

            config.Routes.MapHttpRoute(
                name: "GamePublisherApiLang",
                routeTemplate: "api/{lang}/publisher/{publisherId}/games",
                defaults: new {id = RouteParameter.Optional, lang = "ru", controller = "Publishers"},
                constraints: new {lang = @"ru|en"}
            );

            config.Routes.MapHttpRoute(
                name: "GameApiLang",
                routeTemplate: "api/{lang}/genres/{genreId}/games",
                defaults: new { lang = "ru", controller="Genres" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiLang",
                routeTemplate: "api/{lang}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { lang = @"ru|en" }
            );

            config.Routes.MapHttpRoute(
                name: "GameGenreApi",
                routeTemplate: "api/games/{gameId}/genres",
                defaults: new { id = RouteParameter.Optional, lang = "ru", controller = "Games" }
            );

            config.Routes.MapHttpRoute(
                name: "GamePublisherApi",
                routeTemplate: "api/publisher/{publisherId}/games",
                defaults: new { id = RouteParameter.Optional, lang = "ru", controller = "Publishers" }
            );
            
            config.Routes.MapHttpRoute(
                name: "GameApi",
                routeTemplate: "api/genres/{genreId}/games",
                defaults: new { lang = "ru", controller = "Genres" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, lang = "ru" }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("format", "json", new MediaTypeHeaderValue("application/json")));

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("format", "xml", new MediaTypeHeaderValue("application/xml")));

            GlobalConfiguration.Configuration.Services.RemoveAll(
                typeof(System.Web.Http.Validation.ModelValidatorProvider),
                v => v is InvalidModelValidatorProvider);
        }
    }
}
