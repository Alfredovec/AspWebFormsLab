using System;
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
    public class OrderControllerTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IStoreServices> service;
        private static OrdersController controller;
        private static Mock<IOrderService> orderService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            service = new Mock<IStoreServices>();
            orderService = new Mock<IOrderService>();
            service.Setup(s => s.OrderService).Returns(orderService.Object);
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            controller = new OrdersController(service.Object);
            Mapper.CreateMap<Shipper, ShipperViewModel>();
        }

        [TestMethod]
        public void Get_filter_history_view()
        {
            //Arrange

            //Act
            var res = controller.History() as ViewResult;

            //Assert
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Find_orders()
        {
            //Arrange
            var diaposon = new DateDiaposonViewModel {EndDate = DateTime.UtcNow, StartDate = DateTime.UtcNow};
            var orders = new List<Order>();
            orderService.Setup(o => o.GetHistory(diaposon.StartDate, diaposon.EndDate)).Returns(orders);

            //Act
            var res = controller.FindOrders(diaposon) as PartialViewResult;

            //Assert
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Find_orders_with_not_valid_mode_state()
        {
            //Arrange
            var diaposon = new DateDiaposonViewModel { EndDate = DateTime.UtcNow, StartDate = DateTime.UtcNow };
            controller.ModelState.AddModelError("1", "1");

            //Act
            var res = controller.FindOrders(diaposon) as HttpNotFoundResult;

            //Assert
            Assert.IsNotNull(res);
        }
    }
}
