using GameStore.BLL.Services;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.BLL
{
    [TestClass]
    public class ShipperServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IShipperRepository> repo;
        private static IShipperService shipperService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            shipperService = new StoreService(unitOfWork.Object, log.Object).ShipperService;
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            unitOfWork.ResetCalls();
            repo = new Mock<IShipperRepository>();
            unitOfWork.SetupGet(u => u.ShipperRepository).Returns(repo.Object);
        }

        [TestMethod]
        public void Get_all_shippers()
        {
            //Arrange
            
            //Act
            var res = shipperService.GetAll();

            //Assert
            repo.Verify(r => r.Get(), Times.Once());
        }

        [TestMethod]
        public void Get_shipper_by_id()
        {
            //Arrange
            var shipper = new Shipper {Id = 1};
            repo.Setup(r => r.Get(shipper.Id)).Returns(shipper);

            //Act
            var res = shipperService.Get(shipper.Id);

            //Assert
            Assert.AreEqual(shipper, res);
        }
    }
}
