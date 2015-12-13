using System;
using System.Collections.Generic;
using System.Security.Principal;
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
    public class CommentControllerTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IStoreServices> service;
        private static CommentController controller;
        private static Mock<ICommentService> commentService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            service = new Mock<IStoreServices>();
            commentService = new Mock<ICommentService>();
            service.Setup(s => s.CommentService).Returns(commentService.Object);
            Mapper.CreateMap<CommentViewModel, Comment>();
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            controller = new CommentController(service.Object);
            Mapper.CreateMap<Comment, CommentViewModel>();
            Mapper.CreateMap<CommentViewModel, Comment>();
            var user = new Mock<IIdentity>();
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(p => p.HttpContext.User.Identity).Returns(user.Object);
            controller.ControllerContext = controllerContext.Object;
        }

        [TestMethod]
        public void Get_all_comments()
        {
            //Arrange
            var gameKey = "Key";
            var editinComment = new CommentViewModel();
            var comments = new List<Comment> {new Comment()};
            commentService.Setup(c => c.GetComments(gameKey)).Returns(comments);

            //Act
            var res = controller.Comments(gameKey, editinComment) as ViewResult;

            //Assert
            Assert.AreEqual(1, ((List<CommentViewModel>) res.Model).Count);
        }

        [TestMethod]
        public void Get_all_comments_for_not_found_game()
        {
            //Arrange
            var gameKey = "Key";
            var editinComment = new CommentViewModel();
            commentService.Setup(c => c.GetComments(gameKey)).Throws(new ArgumentException());

            //Act
            var res = controller.Comments(gameKey, editinComment) as HttpNotFoundResult;

            //Assert
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void New_comment_start_view()
        {
            //Arrange
            var comment = new CommentViewModel();
            var error = "error";
            controller.TempData["ModelStateError"] = error;

            //Act
            var res = controller.NewComment(comment) as PartialViewResult;

            //Assert
            Assert.IsNotNull(res);
            Assert.IsFalse(controller.ModelState.IsValid);
        }

        [TestMethod]
        public void Create_new_comment()
        {
            //Arrange
            var gameKey = "Key";
            var comment = new CommentViewModel();
            long parentId = 1;
            long quoteId = 2;

            //Act
            var res = controller.NewComment(gameKey, comment, parentId, quoteId) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual("Comments", res.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_not_valid_new_comment()
        {
            //Arrange
            controller.ModelState.AddModelError("1", "1");
            var comment = new CommentViewModel {Name = "Name", Body = "Body"};

            //Act
            var res = controller.NewComment(null, comment, null, null) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(comment.Name, res.RouteValues["Name"]);
            Assert.AreEqual(comment.Body, res.RouteValues["Body"]);
            Assert.AreEqual("Comments", res.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_new_comment_with_error()
        {
            //Arrange
            var comment = new CommentViewModel { Name = "Name", Body = "Body" };
            commentService.Setup(
                c => c.CreateComment(It.IsAny<Comment>(), It.IsAny<string>(), It.IsAny<List<CommentAction>>()))
                .Throws(new InvalidOperationException());

            //Act
            var res = controller.NewComment(null, comment, null, null) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(res);
            Assert.IsTrue(controller.TempData.ContainsKey("ModelStateError"));
            Assert.AreEqual(comment.Name, res.RouteValues["Name"]);
            Assert.AreEqual(comment.Body, res.RouteValues["Body"]);
            Assert.AreEqual("Comments", res.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_new_comment_for_not_valid_gameKey()
        {
            //Arrange
            var comment = new CommentViewModel { Name = "Name", Body = "Body" };
            commentService.Setup(
                c => c.CreateComment(It.IsAny<Comment>(), It.IsAny<string>(), It.IsAny<List<CommentAction>>()))
                .Throws(new ArgumentException());

            //Act
            var res = controller.NewComment(null, comment, null, null) as HttpNotFoundResult;

            //Assert
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Start_delete_comment()
        {
            //Arrange
            var id = 1;
            var redirectUrl = "url";
            var comment = new Comment();
            commentService.Setup(c => c.GetComment(id)).Returns(comment);

            //Act
            var res = controller.Delete(id, redirectUrl) as ViewResult;

            //Assert
            Assert.AreEqual(typeof (CommentViewModel), res.Model.GetType());
            Assert.AreEqual(redirectUrl, controller.TempData["redirectUrl"]);
        }

        [TestMethod]
        public void Delete_comment()
        {
            //Arrange
            var id = 1;
            var redirectUrl = "url";
            controller.TempData["redirectUrl"] = redirectUrl;

            //Act
            var res = controller.Delete(id) as RedirectResult;

            //Assert
            Assert.AreEqual(redirectUrl, res.Url);
        }

        [TestMethod]
        public void Delete_unexisting_comment()
        {
            //Arrange
            var id = 1;
            commentService.Setup(c => c.DeleteComment(id)).Throws(new ArgumentException());

            //Act
            var res = controller.Delete(id) as HttpNotFoundResult;

            //Assert
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Start_ban_comment()
        {
            //Arrange
            var id = 1;
            var redirectUrl = "url";
            var comment = new Comment();
            commentService.Setup(c => c.GetComment(id)).Returns(comment);

            //Act
            var res = controller.Ban(id, redirectUrl) as ViewResult;

            //Assert
            Assert.AreEqual(typeof(CommentViewModel), res.Model.GetType());
            Assert.AreEqual(redirectUrl, controller.TempData["redirectUrl"]);
        }

        [TestMethod]
        public void Ban_comment()
        {
            //Arrange
            var ban = new BanCommentViewModel
            {
                Ban = BanCommentViewModel.BanDuration.OneDay,
                CommentId = 1
            };
            var redirectUrl = "url";
            controller.TempData["redirectUrl"] = redirectUrl;
            var oneDay = new TimeSpan(1, 0, 0, 0);

            //Act
            var res = controller.Ban(ban) as RedirectResult;

            //Assert
            commentService.Verify(c => c.BanComment(ban.CommentId, oneDay));
            Assert.AreEqual(redirectUrl, res.Url);
        }

        [TestMethod]
        public void Ban_not_valid_comment()
        {
            //Arrange
            var ban = new BanCommentViewModel();
            controller.ModelState.AddModelError("1","1");

            //Act
            var res = controller.Ban(ban) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", res.RouteValues["action"]);
            Assert.AreEqual("Game", res.RouteValues["controller"]);
        }

        [TestMethod]
        public void Ban_comment_for_not_existin_comment()
        {
            //Arrange
            var ban = new BanCommentViewModel
            {
                Ban = BanCommentViewModel.BanDuration.OneDay,
                CommentId = 1
            };
            var redirectUrl = "url";
            var oneDay = new TimeSpan(1, 0, 0, 0);
            commentService.Setup(c => c.BanComment(ban.CommentId, oneDay)).Throws(new ArgumentException());

            //Act
            var res = controller.Ban(ban) as HttpNotFoundResult;

            //Assert
            Assert.IsNotNull(res);
        }
    }
}
