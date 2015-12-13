using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using GameStore.Web.Controllers;
using GameStore.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.Web
{
    [TestClass]
    public class ShipperControllerTest
    {
    
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IStoreServices> service;
        private static ShipperController controller;
        private static Mock<IShipperService> shipperService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            service = new Mock<IStoreServices>();
            shipperService = new Mock<IShipperService>();
            service.Setup(s => s.ShipperService).Returns(shipperService.Object);
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            controller = new ShipperController(service.Object);
            Mapper.CreateMap<Shipper, ShipperViewModel>();
        }

        [TestMethod]
        public void Get_all_shippers()
        {
            //Arrange
            var shippers = new List<Shipper> {new Shipper()};
            shipperService.Setup(s => s.GetAll()).Returns(shippers);

            //Act
            var res = controller.Index() as JsonResult;

            //Assert
            Assert.AreEqual(typeof(List<ShipperViewModel>), res.Data.GetType());
        }

        [TestMethod]
        public void Get_shipper_by_id()
        {
            //Arrange
            var shipper = new Shipper {Id = 1};
            shipperService.Setup(s => s.Get(shipper.Id)).Returns(shipper);

            //Act
            var res = controller.Details(shipper.Id) as JsonResult;

            //Assert
            Assert.AreEqual(typeof(ShipperViewModel), res.Data.GetType());
        }
    }
}
