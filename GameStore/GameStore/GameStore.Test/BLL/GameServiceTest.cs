using System;
using System.Collections.Generic;
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
    public class GameServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IGameRepository> repo;
        private static IGameService gameService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            gameService = new StoreService(unitOfWork.Object, log.Object).GameService;
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            unitOfWork.ResetCalls();
            repo = new Mock<IGameRepository>();
            unitOfWork.SetupGet(u => u.GameRepository).Returns(repo.Object);
        }

        [TestMethod]
        public void Create_New_Game()
        {
            //Arrange
            var game = new Game { Translations = new List<GameTranslation> { new GameTranslation() } };

            //Act
            gameService.CreateGame(game);

            //Assert
            repo.Verify(r => r.Create(game), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once, "Data not saved");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_New_Game_With_Existing_Key()
        {
            //Arrange
            var game = new Game { Translations = new List<GameTranslation> { new GameTranslation() } };
            repo.Setup(r => r.Create(game)).Throws<Exception>();

            //Act
            gameService.CreateGame(game);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Edit_And_Set_Existing_Key_For_Game()
        {
            //Arrange
            var game = new Game { Translations = new List<GameTranslation> { new GameTranslation() } };
            repo.Setup(r => r.Edit(game)).Throws<Exception>();

            //Act
            gameService.EditGame(game);

            //Assert
        }

        [TestMethod]
        public void Edit_Game()
        {
            //Arrange
            var game = new Game { Translations = new List<GameTranslation>{new GameTranslation()} };

            //Act
            gameService.EditGame(game);

            //Assert
            repo.Verify(r => r.Edit(game), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once, "Data not saved");
        }

        [TestMethod]
        public void Delete_existing_game()
        {
            //Arrange
            var id = 1;
            var game = new Game { Id = id, Key = "Game1" };
            repo.Setup(r => r.Get(id)).Returns(game);

            //Act
            gameService.DeleteGame(id);

            //Assert
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [TestMethod]
        public void Get_all_games()
        {
            //Arrange

            //Act
            gameService.GetAllGames();

            //Assert
            repo.Verify(r => r.Get(), Times.Once);
        }

        [TestMethod]
        public void Get_game_by_key()
        {
            //Arrange
            var key = "key";

            //Act
            gameService.GetGame(key);

            //Assert
            repo.Verify(r => r.Get(key), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_unexisting_game_by_key()
        {
            //Arrange
            var key = "key";
            repo.Setup(r => r.Get(It.IsAny<string>())).Throws<InvalidOperationException>();

            //Act
            gameService.GetGame(key);
        }

        [TestMethod]
        public void Get_games_by_genre()
        {
            //Arrange
            var genres = new[]
            {
                new Genre {Id = 1},
                new Genre {Id = 2},
                new Genre {Id = 3}
            };
            var games = new[]
            {
                new Game {Genres = genres, Id = 1},
                new Game {Genres = new[] {genres[0], genres[2]}, Id = 2},
                new Game {Genres = new[] {genres[1], genres[2]}, Id = 3},
                new Game {Genres = new[] {genres[2]}, Id = 4},
                new Game {Genres = new[] {genres[0]}, Id = 5}
            };
            repo.Setup(r => r.Get(It.IsAny<Func<Game, bool>>()))
                .Returns((Func<Game, bool> p) => games.Where(p));
            var findGenreId = 1;
            var result = new[] { games[0], games[1], games[4] };

            //Act
            var res = gameService.GetGameByGenre(findGenreId);

            //Assert
            CollectionAssert.AreEqual(result, res.ToArray());
        }

        [TestMethod]
        public void Get_game_by_platform()
        {
            //Arrange
            var platforms = new[]
            {
                new PlatformType{Id = 1},
                new PlatformType {Id = 2},
                new PlatformType {Id = 3}
            };
            var games = new[]
            {
                new Game {PlatformTypes = platforms, Id = 1},
                new Game {PlatformTypes = new[] {platforms[0], platforms[2]}, Id = 2},
                new Game {PlatformTypes = new[] {platforms[1], platforms[2]}, Id = 3},
                new Game {PlatformTypes = new[] {platforms[2]}, Id = 4},
                new Game {PlatformTypes = new[] {platforms[0]}, Id = 5}
            };
            repo.Setup(r => r.Get(It.IsAny<Func<Game, bool>>()))
                .Returns((Func<Game, bool> p) => games.Where(p));
            var findPlatformId = 1;
            var result = new[] { games[0], games[1], games[4] };

            //Act
            var res = gameService.GetGameByPlatform(findPlatformId);

            //Assert
            CollectionAssert.AreEqual(result, res.ToArray());
        }

        [TestMethod]
        public void Get_count_of_all_game()
        {
            //Arrange

            //Act
            gameService.CountGames();

            //Assert
            repo.Verify(r => r.CountGames(), Times.Once());
        }

        [TestMethod]
        public void Get_max_price()
        {
            //Arrange
            var games = new[] { new Game { Price = 100 }, new Game { Price = 1000 }, new Game { Price = 500 } };
            repo.Setup(r => r.Get()).Returns(games);

            //Act
            var res = gameService.MaxPrice();

            //Assert
            Assert.AreEqual(1000, res);
        }

        [TestMethod]
        public void Try_get_game_by_key()
        {
            //Arrange
            var key = "key";

            //Act
            var res = gameService.GetGame(key);

            //Assert
            repo.Verify(r => r.Get(key), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexisting_game_by_key()
        {
            //Arrange
            var key = "key";
            repo.Setup(r => r.Get(key)).Throws(new InvalidOperationException());

            //Act
            var res = gameService.GetGame(key);

            //Assert
        }

        [TestMethod]
        public void Try_get_game_by_id()
        {
            //Arrange
            var id = 1;

            //Act
            var res = gameService.GetGame(id);

            //Assert
            repo.Verify(r => r.Get(id), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexisting_game_by_id()
        {
            //Arrange
            var id = 1;
            repo.Setup(r => r.Get(id)).Throws(new InvalidOperationException());

            //Act
            var res = gameService.GetGame(id);

            //Assert
        }

        [TestMethod]
        public void Try_view_game_by_key()
        {
            //Arrange
            var game = new Game { Key = "key", Id = 1 };
            repo.Setup(r => r.Get(game.Key)).Returns(game);

            //Act
            gameService.ViewGame(game.Key);

            //Assert
            repo.Verify(r => r.ViewGame(game.Id), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
        }

        private IEnumerable<Game> getGamesForFilter()
        {
            for (int i = 1; i <= 10; i++)
            {
                yield return new Game
                {
                    Name = "Name " + i,
                    Price = i,
                    Comments = new List<Comment> {new Comment()},
                    Publisher = new Publisher {Id = i},
                    PlatformTypes = new List<PlatformType>{new PlatformType{Id = i}},
                    Id = i,
                    Genres = new List<Genre> { new Genre { Id = i } },
                    ViewedCount = i,
                    CreationDate = DateTime.UtcNow
                };
            }
        }

        [TestMethod]
        public void Try_use_empty_filter()
        {
            //Arrange
            var empty = new GameFilterModel { PageNumber = 1, PageSize = 100 };
            repo.Setup(r => r.Get()).Returns(getGamesForFilter());
            var games = getGamesForFilter().Reverse().ToList();

            //Act
            var res = gameService.FilterGames(empty);

            //Assert
            Assert.AreEqual(games[9].Key, res.Games.ToList()[9].Key);
        }

        [TestMethod]
        public void Filter_game_by_name()
        {
            //Arrange
            var searchName = "Name 3";
            var filter = new GameFilterModel
            {
                PageNumber = 1,
                PageSize = 0,
                Name = searchName,
                OrderType = OrderType.ByPriceAsc,
                DateFilter = DateFilter.LastMonth
            };
            repo.Setup(r => r.Get()).Returns(getGamesForFilter());

            //Act
            var res = gameService.FilterGames(filter);

            //Assert
            Assert.AreEqual(1, res.Games.Count());
            Assert.AreEqual(searchName, res.Games.First().Name);
        }

        [TestMethod]
        public void Filter_game_by_price()
        {
            //Arrange
            var minPrice = 3;
            var maxPrice = 4;
            var filter = new GameFilterModel
            {
                PageNumber = 1,
                PageSize = 0,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                OrderType = OrderType.ByPriceDesc,
                DateFilter = DateFilter.LastWeek
            };
            repo.Setup(r => r.Get()).Returns(getGamesForFilter());

            //Act
            var res = gameService.FilterGames(filter);

            //Assert
            Assert.AreEqual(2, res.Games.Count());
        }

        [TestMethod]
        public void Filter_game_by_publisher()
        {
            //Arrange
            var publisherId = 5;
            var filter = new GameFilterModel
            {
                PageNumber = 1,
                PageSize = 0,
                PublishersName = new List<long> {publisherId},
                OrderType = OrderType.MostCommented,
                DateFilter = DateFilter.LastYear
            };
            repo.Setup(r => r.Get()).Returns(getGamesForFilter());

            //Act
            var res = gameService.FilterGames(filter);

            //Assert
            Assert.AreEqual(1, res.Games.Count());
            Assert.AreEqual(publisherId, res.Games.First().Publisher.Id);
        }

        [TestMethod]
        public void Filter_game_by_genres()
        {
            //Arrange
            var genreId = 4;
            var filter = new GameFilterModel
            {
                PageNumber = 1,
                PageSize = 0,
                GenreNames = new List<long> { genreId },
                OrderType = OrderType.New,
                DateFilter = DateFilter.ThreeYear
            };
            repo.Setup(r => r.Get()).Returns(getGamesForFilter());

            //Act
            var res = gameService.FilterGames(filter);

            //Assert
            Assert.AreEqual(1, res.Games.Count());
            Assert.AreEqual(genreId, res.Games.First().Genres.First().Id);
        }

        [TestMethod]
        public void Filter_game_by_platforms()
        {
            //Arrange
            var platformType = 6;
            var filter = new GameFilterModel
            {
                PageNumber = 1,
                PageSize = 0,
                PlatformTypesNames = new List<long> { platformType },
                DateFilter = DateFilter.TwoYear
            };
            repo.Setup(r => r.Get()).Returns(getGamesForFilter());

            //Act
            var res = gameService.FilterGames(filter);

            //Assert
            Assert.AreEqual(1, res.Games.Count());
            Assert.AreEqual(platformType, res.Games.First().PlatformTypes.First().Id);
        }
    }
}
