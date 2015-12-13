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
    public class CommentRepositoryTest
    {
        private static ICommentRepository repository;
        private static Mock<GameStoreContext> dbContext;
        private static IDbSet<Comment> data;

        [TestInitialize]
        public void TestInitiallizer()
        {
            dbContext = new Mock<GameStoreContext>();
            repository = new GameStoreCommentRepository(dbContext.Object);
            data = new FakeDbSet<Comment>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new Comment { Id = i});
            }
            dbContext.SetupGet(c => c.Comments).Returns(data);
        }

        [TestMethod]
        public void Create_comment_type()
        {
            //Arrange
            var commentId = 100;
            var games = new FakeDbSet<Game>
            {
                new Game{Id = commentId}
            };
            var type = new Comment { Id = commentId, Game = games.First()};
            dbContext.Setup(c => c.Games).Returns(games);
            
            dbContext.SetupGet(c => c.Comments).Returns(data);

            //Act
            repository.Create(type);

            //Assert
            Assert.AreEqual(11, data.Count());
            Assert.AreEqual(commentId, type.Game.Id);
            Assert.AreEqual(type, data.First(p => p.Id == commentId));
        }

        [TestMethod]
        public void Edit_comment_type()
        {
            //Arrange
            var commentId = 100;
            var type = new Comment { Id = commentId };

            //Act
            repository.Edit(type);

            //Assert
            dbContext.Verify(c => c.SetState(type, EntityState.Modified), Times.Once);
        }

        [TestMethod]
        public void Delete_comment_type()
        {
            //Arrange
            var commentId = 1;
            var type = data.First(p => p.Id == commentId);

            //Act
            repository.Delete(type);

            //Assert
            Assert.AreEqual(9, data.Count());
            Assert.IsTrue(data.All(p => p.Id != commentId));
        }

        [TestMethod]
        public void Get_comment_type_by_id()
        {
            //Arrange
            var commentId = 1;
            var type = data.First(p => p.Id == commentId);

            //Act
            var res = repository.Get(commentId);

            //Assert
            Assert.AreEqual(type, res);
        }

        [TestMethod]
        public void Get_all_comment_types()
        {
            //Arrange

            //Act
            var res = repository.Get();

            //Assert
            Assert.AreEqual(10, res.Count());
        }

        [TestMethod]
        public void Get_comment_types_with_predicate()
        {
            //Arrange
            var commentId = 1;
            Func<Comment, bool> predicate = p => p.Id == commentId;
            var type = data.First(predicate);

            //Act
            var res = repository.Get(predicate);

            //Assert
            Assert.AreEqual(type, res.First());
        }
    }
}
