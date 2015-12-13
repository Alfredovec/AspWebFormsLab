using System;
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
    public class PublisherControllerTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IStoreServices> service;
        private static PublisherController controller;
        private static Mock<IPublisherService> publisherService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            service = new Mock<IStoreServices>();
            publisherService = new Mock<IPublisherService>();
            service.Setup(s => s.PublisherService).Returns(publisherService.Object);
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            controller = new PublisherController(service.Object, log.Object);

            Mapper.CreateMap<Publisher, PublisherViewModel>();
            Mapper.CreateMap<Publisher, PublisherCreateViewModel>();
            Mapper.CreateMap<PublisherViewModel, Publisher>();
            Mapper.CreateMap<PublisherCreateViewModel, Publisher>();
        }

        [TestMethod]
        public void Start_create_new_publisher()
        {
            //Arrange

            //Act
            var view = controller.New() as ViewResult;

            //Assert
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void Get_detail_of_publisher()
        {
            //Arrange
            var companyName = "name";
            var publisher = new Publisher();
            publisherService.Setup(p => p.GetPublisher(companyName)).Returns(publisher);

            //Act
            var view = controller.Details(companyName) as ViewResult;

            //Assert
            Assert.AreEqual(typeof (PublisherViewModel), view.Model.GetType());
        }

        [TestMethod]
        public void Get_unexisting_publisher()
        {
            //Arrange
            var companyName = "name";
            publisherService.Setup(p => p.GetPublisher(companyName)).Throws<ArgumentException>();

            //Act
            var notFound = controller.Details(companyName) as HttpNotFoundResult;

            //Assert
            Assert.IsNotNull(notFound);
        }

        [TestMethod]
        public void Create_new_publisher()
        {
            //Arrange
            var publisher = new PublisherCreateViewModel{CompanyName = "name"};

            //Act
            var redirect = controller.New(publisher) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual(publisher.CompanyName, redirect.RouteValues["companyName"]);
            publisherService.Verify(p => p.CreatePublisher(It.IsAny<Publisher>()), Times.Once);
        }

        [TestMethod]
        public void Create_not_valid_new_publisher()
        {
            //Arrange
            controller.ModelState.AddModelError("", "");
            var publisher = new PublisherCreateViewModel();

            //Act
            var view = controller.New(publisher) as ViewResult;

            //Assert
            Assert.IsNotNull(view);
            Assert.AreEqual(publisher, view.Model);
        }

        [TestMethod]
        public void Create_new_publisher_with_ununique_name()
        {
            //Arrange
            var publisher = new PublisherCreateViewModel();
            publisherService.Setup(p => p.CreatePublisher(It.IsAny<Publisher>())).Throws(new ArgumentException("1","1"));

            //Act
            var view = controller.New(publisher) as ViewResult;

            //Assert
            Assert.AreEqual(publisher, view.Model);
            Assert.IsFalse(controller.ModelState.IsValid);
        }
    }
}
