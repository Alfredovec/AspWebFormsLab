using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameStore.GameStoreDAL;
using GameStore.GameStoreDAL.RefNavigator;
using GameStore.GameStoreDAL.Repositories;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.GameStoreDAL.Repository
{
    [TestClass]
    public class PlatformTypeRepositoryTest
    {
        private static IPlatformTypeRepository repository;
        private static Mock<GameStoreContext> dbContext;
        private static IDbSet<PlatformType> data;
        
        [TestInitialize]
        public void TestInitiallizer()
        {
            dbContext = new Mock<GameStoreContext>();
            repository = new GameStorePlatformTypeRepository(dbContext.Object);
            data = new FakeDbSet<PlatformType>();
            for(int i =0; i<10; i++)
            {
                data.Add(new PlatformType {Id = i, Type = "Type" + i, Games = new List<Game> {new Game {Id = i}}});
            };
            dbContext.SetupGet(c => c.PlatformTypes).Returns(data);
        }

        [TestMethod]
        public void Create_platform_type()
        {
            //Arrange
            var platformId = 100;
            var type = new PlatformType{Id=platformId};

            //Act
            repository.Create(type);

            //Assert
            Assert.AreEqual(11, data.Count());
            Assert.AreEqual(type, data.First(p=>p.Id==platformId));
        }

        [TestMethod]
        public void Edit_platform_type()
        {
            //Arrange
            var platformId = 100;
            var type = new PlatformType { Id = platformId };

            //Act
            repository.Edit(type);

            //Assert
            dbContext.Verify(c => c.SetState(type, EntityState.Modified), Times.Once);
        }

        [TestMethod]
        public void Delete_platform_type()
        {
            //Arrange
            var platformId = 1;
            var type = data.First(p => p.Id == platformId);

            //Act
            repository.Delete(type);

            //Assert
            Assert.AreEqual(9, data.Count());
            Assert.IsTrue(data.All(p => p.Id != platformId));
        }

        [TestMethod]
        public void Get_platform_type_by_id()
        {
            //Arrange
            var platformId = 1;
            var type = data.First(p => p.Id == platformId);

            //Act
            var res = repository.Get(platformId);

            //Assert
            Assert.AreEqual(type, res);
        }

        [TestMethod]
        public void Get_all_platform_types()
        {
            //Arrange

            //Act
            var res = repository.Get();

            //Assert
            Assert.AreEqual(10, res.Count());
        }

        [TestMethod]
        public void Get_platform_types_with_predicate()
        {
            //Arrange
            var platformId = 1;
            Func<PlatformType, bool> predicate = p => p.Id == platformId;
            var type = data.First(predicate);

            //Act
            var res = repository.Get(predicate);

            //Assert
            Assert.AreEqual(type, res.First());
        }

        [TestMethod]
        public void Get_platform_types_for_game()
        {
            //Arrange
            var gameId = 5;
            var type = data.First(p => p.Id == gameId);

            //Act
            var res = repository.GetPlatformTypesForGame(gameId);

            //Assert
            Assert.AreEqual(type, res.First());
        }
    }
}
