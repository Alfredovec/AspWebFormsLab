using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using GameStore.Web.Controllers;
using GameStore.Web.Infrastructure.PaymentStrategy.Interfaces;
using GameStore.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.Web
{
    [TestClass]
    public class BasketControllerTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IStoreServices> service;
        private static BasketController controller;
        private static Mock<IOrderService> orderService;
        private static Mock<IPaymentService> paymentService;
        private static string currentCustomer = "current";
        private static Mock<IPaymentContext> payContext;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            service = new Mock<IStoreServices>();
            orderService = new Mock<IOrderService>();
            paymentService = new Mock<IPaymentService>();
            service.Setup(s => s.OrderService).Returns(orderService.Object);
            service.Setup(s => s.PaymentService).Returns(paymentService.Object);
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            var user = new Mock<IIdentity>();
            payContext = new Mock<IPaymentContext>();
            controller = new BasketController(service.Object, payContext.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(p => p.HttpContext.User.Identity).Returns(user.Object);
            controller.ControllerContext = controllerContext.Object;
            
            Mapper.CreateMap<Order, OrderViewModel>();
            Mapper.CreateMap<OrderDetail, OrderDetail>();
            Mapper.CreateMap<Payment, PaymentViewModel>();
        }

        [TestMethod]
        public void Get_basket_of_current_user()
        {
            //Arrange
            var order = new Order();
            orderService.Setup(o=>o.GetOrderByCustomerId(It.IsAny<string>())).Returns(order);

            //Act
            var view = controller.Index() as ViewResult;

            //Assert
            Assert.AreEqual(typeof (OrderViewModel), view.Model.GetType());
        }

        [TestMethod]
        public void Get_empty_basket_of_current_user()
        {
            //Arrange
            Order order = null;
            orderService.Setup(o => o.GetOrderByCustomerId(It.IsAny<string>())).Returns(order);

            //Act
            var view = controller.Index() as ViewResult;

            //Assert
            Assert.IsNull(view.Model);
        }

        [TestMethod]
        public void Add_order_detail()
        {
            //Arrange
            var gameKey = "key";
            var gameService = new Mock<IGameService>();
            gameService.Setup(g => g.GetGame(gameKey));
            service.Setup(s => s.GameService).Returns(gameService.Object);

            //Act
            var redirect = controller.Add(gameKey) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", redirect.RouteValues["action"]);
        }

        [TestMethod]
        public void Get_order_of_current_user()
        {
            //Arrange
            var order = new Order{OrderDetails = new List<OrderDetail>()};
            orderService.Setup(o => o.GetOrderByCustomerId(It.IsAny<string>())).Returns(order);

            //Act
            var view = controller.PreparePayment() as ViewResult;

            //Assert
            Assert.AreEqual(typeof(List<OrderDetailViewModel>), view.Model.GetType());
        }

        [TestMethod]
        public void Get_empty_order_of_current_user()
        {
            //Arrange
            Order order = null;
            orderService.Setup(o => o.GetOrderByCustomerId(It.IsAny<string>())).Returns(order);

            //Act
            var view = controller.PreparePayment() as ViewResult;

            //Assert
            Assert.IsNull(view.Model);
        }

        [TestMethod]
        public void Prepare_payment()
        {
            //Arrange
            var order = new Order();
            orderService.Setup(o => o.GetOrderByCustomerId(It.IsAny<string>())).Returns(order);

            //Act
            controller.PreparePayment(PaymentType.Visa);

            //Assert
            payContext.Verify(p => p.Pay(order), Times.Once);
        }

        [TestMethod]
        public void Get_payments()
        {
            //Arrange
            var payments = new List<Payment> { new Payment() };
            paymentService.Setup(p => p.GetAllPayments()).Returns(payments);

            //Act
            var res = controller.Payments() as PartialViewResult;

            //Assert
            Assert.AreEqual(1, ((List<PaymentViewModel>)res.Model).Count);
        }
    }
}
