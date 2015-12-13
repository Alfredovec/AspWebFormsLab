using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.GameStoreDAL;
using GameStore.GameStoreDAL.Repositories;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.GameStoreDAL.Repository
{
    [TestClass]
    public class PaymentRepositoryTest
    {
        private static IPaymentRepository repository;
        private static Mock<GameStoreContext> dbContext;
        private static IDbSet<Payment> data;

        [TestInitialize]
        public void TestInitiallizer()
        {
            dbContext = new Mock<GameStoreContext>();
            repository = new GameStorePaymentRepository(dbContext.Object);
            data = new FakeDbSet<Payment>
            {
                new Payment {Id = 1, Description = "1", Name = "Pay" + 1, Type = PaymentType.Bank},
                new Payment {Id = 2, Description = "2", Name = "Pay" + 2, Type = PaymentType.Ibox},
                new Payment {Id = 2, Description = "2", Name = "Pay" + 2, Type = PaymentType.Visa}
            };
            dbContext.SetupGet(c => c.Payments).Returns(data);
        }

        [TestMethod]
        public void Create_payment_type()
        {
            //Arrange
            var paymentId = 100;
            var type = new Payment { Id = paymentId };

            //Act
            repository.Create(type);

            //Assert
            Assert.AreEqual(4, data.Count());
            Assert.AreEqual(type, data.First(p => p.Id == paymentId));
        }

        [TestMethod]
        public void Edit_payment_type()
        {
            //Arrange
            var paymentId = 100;
            var type = new Payment { Id = paymentId };

            //Act
            repository.Edit(type);

            //Assert
            dbContext.Verify(c => c.SetState(type, EntityState.Modified), Times.Once);

        }

        [TestMethod]
        public void Delete_payment_type()
        {
            //Arrange
            var paymentId = 1;
            var type = data.First(p => p.Id == paymentId);

            //Act
            repository.Delete(type);

            //Assert
            Assert.AreEqual(2, data.Count());
            Assert.IsTrue(data.All(p => p.Id != paymentId));
        }

        [TestMethod]
        public void Get_payment_type_by_id()
        {
            //Arrange
            var paymentId = 1;
            var type = data.First(p => p.Id == paymentId);

            //Act
            var res = repository.Get(paymentId);

            //Assert
            Assert.AreEqual(type, res);
        }

        [TestMethod]
        public void Get_all_payment_types()
        {
            //Arrange

            //Act
            var res = repository.Get();

            //Assert
            Assert.AreEqual(3, res.Count());
        }

        [TestMethod]
        public void Get_payment_types_with_predicate()
        {
            //Arrange
            var paymentId = 1;
            Func<Payment, bool> predicate = p => p.Id == paymentId;
            var type = data.First(predicate);

            //Act
            var res = repository.Get(predicate);

            //Assert
            Assert.AreEqual(type, res.First());
        }
    }
}
