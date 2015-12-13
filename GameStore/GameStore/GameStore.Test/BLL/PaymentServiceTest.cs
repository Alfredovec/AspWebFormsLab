using System;
using System.Linq;
using GameStore.BLL.Services;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.BLL
{
    [TestClass]
    public class PaymentServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IPaymentRepository> repo;
        private static IPaymentService paymentService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            paymentService = new StoreService(unitOfWork.Object, log.Object).PaymentService;
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            unitOfWork.ResetCalls();
            repo = new Mock<IPaymentRepository>();
            unitOfWork.SetupGet(u => u.PaymentRepository).Returns(repo.Object);
        }

        [TestMethod]
        public void Get_all_payment_types()
        {
            //Arrange

            //Act
            var res = paymentService.GetAllPayments();

            //Assert
            repo.Verify(r => r.Get(), Times.Once);
        }

        [TestMethod]
        public void Try_get_payment_by_id()
        {
            //Arrange
            var id = 1;

            //Act
            var res = paymentService.GetPayment(id);

            //Assert
            repo.Verify(r => r.Get(id), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexisting_payment_by_id()
        {
            //Arrange
            var id = 1;
            repo.Setup(r => r.Get(id)).Throws(new InvalidOperationException());

            //Act
            var res = paymentService.GetPayment(id);

            //Assert
        }

        [TestMethod]
        public void Try_get_payment_by_type()
        {
            //Arrange
            var payment = new Payment{Type = PaymentType.Visa};
            var payments = new[] {new Payment(), payment, new Payment()};
            repo.Setup(r => r.Get(It.IsAny<Func<Payment, bool>>()))
                .Returns((Func<Payment, bool> f) => payments.Where(f));

            //Act
            var res = paymentService.GetPayment(payment.Type);

            //Assert
            Assert.AreEqual(payment, res);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexisting_payment_by_type()
        {
            //Arrange
            var type = PaymentType.Visa;
            repo.Setup(r => r.Get(It.IsAny<Func<Payment, bool>>())).Returns(Enumerable.Empty<Payment>());

            //Act
            var res = paymentService.GetPayment(type);

            //Assert
        }

        [TestMethod]
        public void Try_create_valid_payment()
        {
            //Arrange
            var payment = new Payment();

            //Act
            paymentService.CreatePayment(payment);

            //Assert
            repo.Verify(r => r.Create(payment), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_create_unvalid_payment()
        {
            //Arrange
            var payment = new Payment();
            repo.Setup(r => r.Create(payment)).Throws(new InvalidOperationException());

            //Act
            paymentService.CreatePayment(payment);

            //Assert
        }
    }
}
