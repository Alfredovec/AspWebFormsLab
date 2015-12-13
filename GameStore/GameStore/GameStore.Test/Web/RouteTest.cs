using System;
using System.Web;
using System.Web.Routing;
using GameStore.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.Web
{
    [TestClass]
    public class RouteTest
    {
        private static RouteCollection routes;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
        }

        private static void AssertRoute(RouteCollection routeCollection, string url, object expectations)
        {
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath)
                .Returns(url);

            RouteData routeData = routeCollection.GetRouteData(httpContextMock.Object);
            Assert.IsNotNull(routeData);

            foreach (var kvp in new RouteValueDictionary(expectations))
            {
                Assert.IsTrue(string.Equals(kvp.Value.ToString(),
                                          routeData.Values[kvp.Key].ToString(),
                                          StringComparison.OrdinalIgnoreCase)
                            , string.Format("Expected '{0}', not '{1}' for '{2}'.",
                                            kvp.Value, routeData.Values[kvp.Key], kvp.Key));
            }
        }

        [TestMethod]
        public void Go_to_default_page()
        {
            AssertRoute(routes, "~/",
                new { controller = "Game", action = "Index" });
        }

        [TestMethod]
        public void Get_all_games()
        {
            AssertRoute(routes, "~/games",
                new { controller = "Game", action = "Index" });
        }

        [TestMethod]
        public void Get_game()
        {
            AssertRoute(routes, "~/game/key",
                new { controller = "Game", action = "Get", gameKey = "key" });
        }

        [TestMethod]
        public void Create_Comment()
        {
            AssertRoute(routes, "~/game/key/newcomment",
                new { controller = "Comment", action = "NewComment", gameKey = "key" });
        }

        [TestMethod]
        public void Comments_routes()
        {
            AssertRoute(routes, "~/game/key/action",
                new { controller = "Comment", action = "action", gameKey = "key" });
        }

        [TestMethod]
        public void Donwload_Game()
        {
            AssertRoute(routes, "~/game/key/download",
                new { controller = "Game", action = "download", gameKey = "key" });
        }

        [TestMethod]
        public void Create_Game()
        {
            AssertRoute(routes, "~/games/new",
                   new { controller = "Game", action = "New" });
        }

        [TestMethod]
        public void Edit_Game()
        {
            AssertRoute(routes, "~/games/update",
                new { controller = "Game", action = "Update" });
        }

        [TestMethod]
        public void Get_publisher_details()
        {
            AssertRoute(routes, "~/publisher/name",
                new { controller = "Publisher", action = "Details", companyName = "name" });
        }

        [TestMethod]
        public void Create_new_publisher()
        {
            AssertRoute(routes, "~/publisher/new",
                new { controller = "Publisher", action = "New" });
        }

        [TestMethod]
        public void Get_all_publishers()
        {
            AssertRoute(routes, "~/publisher/all",
                new { controller = "Publisher", action = "All" });
        }

        [TestMethod]
        public void Add_game_to_basket()
        {
            AssertRoute(routes, "~/game/key/buy",
                new { controller = "Basket", action = "Add", gamekey = "key" });
        }

        [TestMethod]
        public void View_basket()
        {
            AssertRoute(routes, "~/basket",
                new { controller = "Basket", action = "Index" });
        }

        [TestMethod]
        public void View_order()
        {
            AssertRoute(routes, "~/order",
                new { controller = "Basket", action = "PreparePayment" });
        }
    }
}
