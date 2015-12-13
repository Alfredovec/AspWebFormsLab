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
    public class CommentServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<ICommentRepository> repo;
        private static ICommentService commentService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            commentService = new StoreService(unitOfWork.Object, log.Object).CommentService;
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            unitOfWork.ResetCalls();
            repo = new Mock<ICommentRepository>();
            unitOfWork.SetupGet(u => u.CommentRepository).Returns(repo.Object);
        }

        [TestMethod]
        public void Create_New_Comment()
        {
            //Arrange
            var comment = new Comment { User = new User() };
            var game = new Game { Key = "Key" };
            var gameRepo = new Mock<IGameRepository>();
            var userRepo = new Mock<IUserRepository>();
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);
            var empty = new List<CommentAction>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Returns(game);

            //Act
            commentService.CreateComment(comment, game.Key, empty);

            //Assert
            Assert.AreEqual(game, comment.Game);
            repo.Verify(r => r.Create(comment), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once, "Data not saved");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_New_Comment_For_not_Existing_Game()
        {
            //Arrange
            var comment = new Comment { User = new User() };
            var game = new Game { Key = "Key" };
            var gameRepo = new Mock<IGameRepository>();
            var empty = new List<CommentAction>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Throws<InvalidOperationException>();

            //Act
            commentService.CreateComment(comment, game.Key, empty);

            //Assert
        }

        [TestMethod]
        public void Reply_to_comment()
        {
            //Arrange
            var parent = new Comment {Id = 1};
            var comment = new Comment { User = new User() };
            var game = new Game {Key = "key"};
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Returns(game);
            repo.Setup(r => r.Get(parent.Id)).Returns(parent);
            var actions = new List<CommentAction>
            {
                new CommentAction
                {
                    ActionEnum = CommentAction.CommentActionEnum.Reply,
                    CommentActionId = parent.Id
                }
            };

            //Act
            commentService.CreateComment(comment, game.Key, actions);

            //Assert
            Assert.AreEqual(parent, comment.Parent);
            Assert.AreEqual(game, comment.Game);
            repo.Verify(r => r.Create(comment), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once, "Data not saved");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Reply_to_comment_For_not_Existing_Game()
        {
            //Arrange
            var parent = new Comment { Id = 1 };
            var comment = new Comment { User = new User() };
            var game = new Game { Key = "key" };
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            var userRepo = new Mock<IUserRepository>();
            unitOfWork.SetupGet(u => u.UserRepository).Returns(userRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Throws<InvalidOperationException>();
            repo.Setup(r => r.Get(parent.Id)).Returns(parent);
            var actions = new List<CommentAction>
            {
                new CommentAction
                {
                    ActionEnum = CommentAction.CommentActionEnum.Reply,
                    CommentActionId = parent.Id
                }
            };

            //Act
            commentService.CreateComment(comment, game.Key, actions);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reply_to_deleted_comment()
        {
            //Arrange
            var parent = new Comment { Id = 1, IsDeleted = true};
            var comment = new Comment{User = new User()};
            var game = new Game { Key = "key" };
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Returns(game);
            repo.Setup(r => r.Get(parent.Id)).Returns(parent);
            var actions = new List<CommentAction>
            {
                new CommentAction
                {
                    ActionEnum = CommentAction.CommentActionEnum.Reply,
                    CommentActionId = parent.Id
                }
            };
            var userRepo = new Mock<IUserRepository>();
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);

            //Act
            commentService.CreateComment(comment, game.Key, actions);

            //Assert
        }

        [TestMethod]
        public void Get_all_comments_for_selected_key()
        {
            //Arrange
            var game = new Game { Id = 1, Key = "1" };
            var parent = new Comment { GameId = 1, Name = "1" };
            var comments = new[]
            {
                new Comment{ GameId = 2, Body="4"},
                new Comment{ GameId = 1, Body="3"},
            };
            parent.Children = new[] { comments[0], comments[1] };
            var allComments = new Comment[comments.Length + 1];
            allComments[0] = parent;
            for (int i = 0; i < comments.Length; i++)
            {
                allComments[i + 1] = comments[i];
            }

            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Returns(game);
            repo.Setup(r => r.GetFirstCommentsInThred(game.Id))
                .Returns(new List<Comment>{parent});

            var result = new[] { parent };

            //Act
            var res = commentService.GetComments(game.Key);

            //Assert
            CollectionAssert.AreEqual(result, res.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_all_comments_for_For_not_Existing_Game()
        {
            //Arrange
            var comment = new Comment();
            var game = new Game { Key = "Key" };
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Throws<InvalidOperationException>();

            //Act
            var res = commentService.GetComments(game.Key);

            //Assert
        }

        [TestMethod]
        public void Quote_comment()
        {
            //Arrange
            var quote = new Comment { Id = 1 };
            var comment = new Comment { User = new User() };
            var game = new Game { Key = "key" };
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Returns(game);
            repo.Setup(r => r.Get(quote.Id)).Returns(quote);
            var actions = new List<CommentAction>
            {
                new CommentAction
                {
                    ActionEnum = CommentAction.CommentActionEnum.Quote,
                    CommentActionId = quote.Id
                }
            };
            var userRepo = new Mock<IUserRepository>();
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);

            //Act
            commentService.CreateComment(comment, game.Key, actions);

            //Assert
            Assert.AreEqual(quote, comment.Quote);
            Assert.AreEqual(game, comment.Game);
            repo.Verify(r => r.Create(comment), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once, "Data not saved");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Quote_comment_For_not_Existing_Game()
        {
            //Arrange
            var quote = new Comment { Id = 1 };
            var comment = new Comment { User = new User() };
            var game = new Game { Key = "key" };
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Throws<InvalidOperationException>();
            repo.Setup(r => r.Get(quote.Id)).Returns(quote);
            var actions = new List<CommentAction>
            {
                new CommentAction
                {
                    ActionEnum = CommentAction.CommentActionEnum.Quote,
                    CommentActionId = quote.Id
                }
            };
            var userRepo = new Mock<IUserRepository>();
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);

            //Act
            commentService.CreateComment(comment, game.Key, actions);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Quote_deleted_comment()
        {
            //Arrange
            var quote = new Comment { Id = 1, IsDeleted = true };
            var comment = new Comment { User = new User() };
            var game = new Game { Key = "key" };
            var gameRepo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(gameRepo.Object);
            gameRepo.Setup(g => g.Get(game.Key)).Returns(game);
            repo.Setup(r => r.Get(quote.Id)).Returns(quote);
            var actions = new List<CommentAction>
            {
                new CommentAction
                {
                    ActionEnum = CommentAction.CommentActionEnum.Quote,
                    CommentActionId = quote.Id
                }
            };
            var userRepo = new Mock<IUserRepository>();
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);

            //Act
            commentService.CreateComment(comment, game.Key, actions);

            //Assert
        }

        [TestMethod]
        public void Ban_comment()
        {
            //Arrange
            var comment = new Comment{Id = 1, User = new User()};
            repo.Setup(r => r.Get(comment.Id)).Returns(comment);
            var period = new TimeSpan();
            var userRepo = new Mock<IUserRepository>();
            unitOfWork.Setup(u => u.UserRepository).Returns(userRepo.Object);

            //Act
            commentService.BanComment(comment.Id, period);

            //Arrange
            unitOfWork.Verify(u => u.Save(), Times.Once());
            userRepo.Verify(u=>u.BanUser(comment.User.Email, period), Times.Once());
        }

        [TestMethod]
        public void Try_get_comment()
        {
            //Arrange
            var comment = new Comment {Id = 1};
            repo.Setup(r => r.Get(comment.Id)).Returns(comment);

            //Act
            var res = commentService.GetComment(comment.Id);

            //Assert
            Assert.AreEqual(comment, res);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexisting_comment()
        {
            //Arrange
            repo.Setup(r => r.Get(It.IsAny<long>())).Throws<InvalidOperationException>();

            //Act
            var res = commentService.GetComment(0);

            //Assert
        }

        [TestMethod]
        public void Try_delete_comment_with_childs()
        {
            //Arrange
            var comment = new Comment
            {
                Id = 1,
                Children = new List<Comment> {new Comment()}
            };
            repo.Setup(r => r.Get(comment.Id)).Returns(comment);

            //Act
            commentService.DeleteComment(comment.Id);

            //Assert
            Assert.IsTrue(comment.IsDeleted);
            repo.Verify(r => r.Delete(comment), Times.Never);
            repo.Verify(r => r.Edit(comment), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void Try_delete_comment_with_quotes()
        {
            //Arrange
            var comment = new Comment
            {
                Id = 1,
                Children = new List<Comment>(),
                Quotes = new List<Comment> {new Comment()}
            };
            repo.Setup(r => r.Get(comment.Id)).Returns(comment);

            //Act
            commentService.DeleteComment(comment.Id);

            //Assert
            Assert.IsTrue(comment.IsDeleted);
            repo.Verify(r => r.Delete(comment), Times.Never);
            repo.Verify(r => r.Edit(comment), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void Try_delete_comment_without_quotes_and_parent()
        {
            //Arrange
            var comment = new Comment
            {
                Id = 1,
                Children = new List<Comment>(),
                Quotes = new List<Comment>()
            };
            repo.Setup(r => r.Get(comment.Id)).Returns(comment);

            //Act
            commentService.DeleteComment(comment.Id);

            //Assert
            repo.Verify(r => r.Delete(comment), Times.Once);
            repo.Verify(r => r.Edit(comment), Times.Never);
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}
