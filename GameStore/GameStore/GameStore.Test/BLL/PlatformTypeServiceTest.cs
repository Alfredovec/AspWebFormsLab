using System;
using GameStore.BLL.Services;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.BLL
{
    [TestClass]
    public class PlatformTypeServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IPlatformTypeRepository> repo;
        private static IStoreServices storeServices;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            repo = new Mock<IPlatformTypeRepository>();
            storeServices = new StoreService(unitOfWork.Object, log.Object);
        }

        [TestMethod]
        public void Try_get_platform_type_by_id()
        {
            //Arrange
            var platformTypeService = storeServices.PlatformTypeService;
            unitOfWork.SetupGet(u => u.PlatformTypeRepository).Returns(repo.Object);

            //Act
            var resultPlatform = platformTypeService.GetPlatformType(1);

            //Assert
            repo.Verify(g => g.Get(It.IsAny<long>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexesting_platform_type_by_id()
        {
            //Arrange
            var platformTypeService = storeServices.PlatformTypeService;
            unitOfWork.SetupGet(u => u.PlatformTypeRepository).Returns(repo.Object);
            repo.Setup(r => r.Get(1)).Throws<InvalidOperationException>();

            //Act
            var resultPlatform = platformTypeService.GetPlatformType(1);

            //Assert
        }

        [TestMethod]
        public void Try_get_platform_type_by_game_id()
        {
            //Arrange
            var platformTypeService = storeServices.PlatformTypeService;
            unitOfWork.SetupGet(u => u.PlatformTypeRepository).Returns(repo.Object);

            //Act
            var resultPlatforms = platformTypeService.GetPlatformTypesForGame(1);

            //Assert
            repo.Verify(g => g.GetPlatformTypesForGame(It.IsAny<long>()));
        }

        [TestMethod]
        public void Try_get_all_platforms()
        {
            //Arrange
            var platformTypeService = storeServices.PlatformTypeService;
            unitOfWork.SetupGet(u => u.PlatformTypeRepository).Returns(repo.Object);

            //Act
            platformTypeService.GetAllPlatformTypes();

            //Assert
            repo.Verify(p=>p.Get(),Times.Once());
        }
    }
}
