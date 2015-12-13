using System;
using System.Collections.Generic;
using System.Linq;
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
    public class OrderServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IOrderRepository> repo;
        private static IOrderService orderService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            orderService = new StoreService(unitOfWork.Object, log.Object).OrderService;
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            unitOfWork.ResetCalls();
            repo = new Mock<IOrderRepository>();
            unitOfWork.SetupGet(u => u.OrderRepository).Returns(repo.Object);
        }

        [TestMethod]
        public void Add_Order_Detail()
        {
            //Arrange
            string orderCustomerId = "id";
            OrderDetail detail = new OrderDetail();

            //Act
            orderService.AddOrderDetail(orderCustomerId, detail);

            //Arrange
            repo.Verify(r => r.AddOrderDetails(orderCustomerId, detail), Times.Once());

        }

        [TestMethod]
        public void Add_new_order_detail()
        {
            //Arrange
            var quantity = 1;
            var customerId = "id";
            var game = new Game { Id = 1 };
            OrderDetail orderDetail = null;
            repo.Setup(r => r.Get(customerId, game.Id)).Returns(orderDetail);

            //Act
            orderService.AddOrderDetail(customerId, game);

            //Assert
            unitOfWork.Verify(u => u.Save(), Times.Once());
            repo.Verify(r => r.AddOrderDetails(customerId, 
                It.Is<OrderDetail>(o=>o.ProductId==game.Id && o.Quantity==quantity)), Times.Once());
        }

        [TestMethod]
        public void Add_not_new_order_detail()
        {
            //Arrange
            var quantity = 5;
            var customerId = "id";
            var game = new Game {Id = 1};
            var orderDetail = new OrderDetail {Quantity = 5};
            repo.Setup(r => r.Get(customerId, game.Id)).Returns(orderDetail);

            //Act
            orderService.AddOrderDetail(customerId, game);

            //Assert
            unitOfWork.Verify(u=>u.Save(),Times.Once());
            repo.Verify(r => r.EditOrderDetail(orderDetail), Times.Once());
            Assert.AreEqual(quantity + 1, orderDetail.Quantity);
        }

        [TestMethod]
        public void Remove_ten_order_detail()
        {
            //Arrange
            short quantity = 5;
            var customerId = "id";
            var game = new Game { Id = 1 };
            var orderDetail = new OrderDetail {Id = 1, Quantity = 5 };
            repo.Setup(r => r.Get(customerId, game.Id)).Returns(orderDetail);

            //Act
            orderService.AddOrderDetail(customerId, game, (short)-quantity);

            //Assert
            unitOfWork.Verify(u => u.Save(), Times.Once());
            repo.Verify(r => r.DeleteOrderDetails(orderDetail.Id), Times.Once());
        }

        [TestMethod]
        public void Get_Order_By_existing_customer_Id()
        {
            //Arrange
            var id = "1";
            var order = new Order {OrderDetails = new List<OrderDetail> {new OrderDetail()}};
            repo.Setup(r => r.Get(id)).Returns(order);
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.Setup(u => u.GameRepository).Returns(gameRepo.Object);

            //Act
            var result = orderService.GetOrderByCustomerId(id);
            
            //Assert
            Assert.AreEqual(order, result);
        }

        [TestMethod]
        public void Get_Order_By_unexisting_customer_Id()
        {
            //Arrange
            var id = "1";
            Order order = null;
            repo.Setup(r => r.Get(id)).Returns(order);

            //Act
            var result = orderService.GetOrderByCustomerId(id);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Try_get_pdf_for_order()
        {
            //Arrange
            var order = new Order {Id=1, OrderDetails = new List<OrderDetail>()};
            repo.Setup(r => r.Get(order.Id)).Returns(order);

            //Act
            var res = orderService.GetPdfForOrder(order.Id);

            //Assert
            Assert.AreNotEqual(0, res.Length);
        }

        [TestMethod]
        public void Get_order_sum()
        {
            //Arrange
            var game = new Game {Id = 1, Price = 100};
            short quantity = 10;
            var order = new Order
            {
                Id = 1,
                OrderDetails = new List<OrderDetail> {new OrderDetail {ProductId = game.Id, Quantity = quantity, Game =  game}}
            };
            repo.Setup(r => r.Get(order.Id)).Returns(order);

            //Act
            var sum = orderService.OrderSum(order.Id);

            //Assert
            Assert.AreEqual(game.Price*quantity, sum);
        }

        [TestMethod]
        public void Get_order_history()
        {
            //Arrange
            var productId = 1;
            var orders = new[]{new Order
            {
                Date = new DateTime(2000,10,10),
                OrderDetails = new List<OrderDetail> {new OrderDetail{ProductId = productId}}
            },
            new Order
            {
                Date = new DateTime(1500,10,10),
                OrderDetails = new List<OrderDetail> {new OrderDetail{ProductId = productId, Game = new Game{Id = productId}}}
            },
            new Order
            {
                Date = new DateTime(2100,10,10),
                OrderDetails = new List<OrderDetail> {new OrderDetail{ProductId = productId, Game = new Game()}}
            }};
            repo.Setup(r => r.Get(It.IsAny<Func<Order, bool>>())).Returns((Func<Order, bool> p) => orders.Where(p));
            var gameRepo = new Mock<IGameRepository>();
            gameRepo.Setup(g => g.Get()).Returns(new List<Game>{new Game{Id = productId}});
            unitOfWork.Setup(u => u.GameRepository).Returns(gameRepo.Object);
            var startDate = new DateTime(1000, 10, 10);
            var endDate = new DateTime(2001, 10, 10);

            //Act
            var result = orderService.GetHistory(startDate, endDate);
            
            //Assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetOrder()
        {
            //Arrange
            var order = new Order {Id = 1};
            repo.Setup(r => r.Get(order.Id)).Returns(order);

            //Act
            var res = orderService.GetOrder(order.Id);

            //Assert
            Assert.AreEqual(order, res);
        }

        [TestMethod]
        public void ShippOrder()
        {
            //Arrange
            var id = 1;

            //Act
            orderService.ShippOrder(id);

            //Assert
            repo.Verify(r => r.ShippOrder(id), Times.Once());
            unitOfWork.Verify(u => u.Save(), Times.Once());
        }
    }
}
