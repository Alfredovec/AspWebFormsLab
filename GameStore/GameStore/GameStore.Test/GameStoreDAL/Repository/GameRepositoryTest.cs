using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.GameStoreDAL;
using GameStore.GameStoreDAL.Repositories;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.GameStoreDAL.Repository
{
    [TestClass]
    public class GameRepositoryTest
    {
        private static IGameRepository repository;
        private static Mock<GameStoreContext> dbContext;
        private static Mock<IStoreOuterRefNavigators> refNavigators; 
        private static IDbSet<Game> data;
        private static Mock<IRefNavigator<Game>> gameRefNavigator;

        [TestInitialize]
        public void TestInitiallizer()
        {
            dbContext = new Mock<GameStoreContext>();
            gameRefNavigator = new Mock<IRefNavigator<Game>>();
            refNavigators = new Mock<IStoreOuterRefNavigators>();
            refNavigators.Setup(r => r.GameRefNavigator).Returns(gameRefNavigator.Object);
            repository = new GameStoreGameRepository(dbContext.Object, refNavigators.Object);
            data = new FakeDbSet<Game>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new Game
                {
                    Id = i, 
                    Key = "key",
                    ContentType = "type"
                });
            }
            dbContext.SetupGet(c => c.Games).Returns(data);
        }

        [TestMethod]
        public void Edit_game()
        {
            //Arrange
            var game = new Game {Id = 1, Key = "keeey", ContentType = "type1"};
            gameRefNavigator.Setup(g => g.DecodeGlobalId(game.Id)).Returns(new RefNavigation {OriginId = game.Id});

            //Act
            repository.Edit(game);

            //Assert
            Assert.AreEqual(game.Key, data.Find(game.Id).Key);
            Assert.AreEqual(game.ContentType, data.Find(game.Id).ContentType);
        }

        [TestMethod]
        public void Delete_game()
        {
            //Arrange
            var gameId = 1;

            //Act
            repository.Delete(gameId);

            //Assert
            Assert.IsTrue(data.First(g=>g.Id==gameId).IsDeleted);
        }

        [TestMethod]
        public void Count_games()
        {
            //Arrange
            
            //Act
            var res = repository.CountGames();

            //Assert
            Assert.AreEqual(data.LongCount(), res);
        }

        [TestMethod]
        public void Get_game_by_id()
        {
            //Arrange
            var gameId = 1;
            gameRefNavigator.Setup(g => g.GenerateGlobalId(gameId, DatabaseName.GameStore)).Returns(gameId);

            //Act
            var res = repository.Get(gameId);

            //Assert
            Assert.AreEqual(gameId, res.Id);
        }

        [TestMethod]
        public void Get_games()
        {
            //Arrange

            //Act
            var games = repository.Get();

            //Assert
            Assert.AreEqual(data.Count(), games.Count());
        }
    }
}
