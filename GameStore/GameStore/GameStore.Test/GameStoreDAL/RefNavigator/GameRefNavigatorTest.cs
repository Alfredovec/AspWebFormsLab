using GameStore.BLL.Services;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.GameStoreDAL;
using GameStore.GameStoreDAL.RefNavigator;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.GameStoreDAL.RefNavigator
{
    [TestClass]
    public class GameRefNavigatorTest
    {
        private static IRefNavigator<Game> refNavigator;
        private static Mock<GameStoreContext> dbContext;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            dbContext = new Mock<GameStoreContext>();
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            var storeNavigators = new StoreOuterRefsNavigators(dbContext.Object);
            refNavigator = storeNavigators.GameRefNavigator;
        }


    }
}
