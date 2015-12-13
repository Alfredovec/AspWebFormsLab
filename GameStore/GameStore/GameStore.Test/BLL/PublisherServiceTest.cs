using System;
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
    public class PublisherServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IPublisherRepository> repo;
        private static IPublisherService publisherService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            publisherService = new StoreService(unitOfWork.Object, log.Object).PublisherService;
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            unitOfWork.ResetCalls();
            repo = new Mock<IPublisherRepository>();
            unitOfWork.SetupGet(u => u.PublisherRepository).Returns(repo.Object);
        }

        [TestMethod]
        public void Try_get_all_publishers()
        {
            //Arrange

            //Act
            publisherService.GetAllPublishers();

            //Assert
            repo.Verify(r => r.Get(), Times.Once());
        }

        [TestMethod]
        public void Try_get_exist_publisher_by_id()
        {
            //Arrange
            long id = 1;

            //Act
            publisherService.GetPublisher(id);

            //Assert
            repo.Verify(r => r.Get(id), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexist_publisher_by_id()
        {
            //Arrange
            long id = 1;
            repo.Setup(r => r.Get(id)).Throws<InvalidOperationException>();

            //Act
            publisherService.GetPublisher(id);

            //Assert
        }

        [TestMethod]
        public void Try_create_correct_publisher()
        {
            //Arrange
            var publisher = new Publisher();

            //Act
            publisherService.CreatePublisher(publisher);

            //Assert
            repo.Verify(r => r.Create(publisher), Times.Once());
            unitOfWork.Verify(u => u.Save(), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_create_uncorrect_publisher()
        {
            //Arrange
            var publisher = new Publisher();
            repo.Setup(r => r.Create(publisher)).Throws<Exception>();

            //Act
            publisherService.CreatePublisher(publisher);

            //Assert
        }

        [TestMethod]
        public void Try_get_existing_publisher_by_company_name()
        {
            //Arrange
            var publishers = new[]
            {
                new Publisher {CompanyName = "p1"},
                new Publisher {CompanyName = "p2"},
                new Publisher {CompanyName = "p3"},
                new Publisher {CompanyName = "p4"},
            };
            var needFind = "p2";
            repo.Setup(r => r.Get(It.IsAny<Func<Publisher, bool>>())).Returns((Func<Publisher, bool> p) => publishers.Where(p));

            //Act
            var result = publisherService.GetPublisher(needFind);

            //Assert
            Assert.AreEqual(result, publishers[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexisting_publisher_by_company_name()
        {
            //Arrange
            var publishers = new[]
            {
                new Publisher {CompanyName = "p1"},
                new Publisher {CompanyName = "p2"},
                new Publisher {CompanyName = "p3"},
                new Publisher {CompanyName = "p4"},
            };
            var needFind = "p5";
            repo.Setup(r => r.Get(It.IsAny<Func<Publisher, bool>>())).Returns((Func<Publisher, bool> p) => publishers.Where(p));

            //Act
            var result = publisherService.GetPublisher(needFind);

            //Assert
        }

        [TestMethod]
        public void Edit_publisher()
        {
            //Arrange
            var publisher = new Publisher();

            //Act
            publisherService.EditPublisher(publisher);

            //Assert
            repo.Verify(r=>r.Edit(publisher),Times.Once());
            unitOfWork.Verify(u => u.Save(), Times.Once());
        }

        [TestMethod]
        public void Remove_publisher()
        {
            //Arrange
            var publisher = new Publisher
            {
                Id = 1
            };
            repo.Setup(r => r.Get(publisher.Id)).Returns(publisher);

            //Act
            publisherService.RemovePublisher(publisher.Id);

            //Assert
            repo.Verify(r => r.Delete(publisher), Times.Once());
        }
    }
}
