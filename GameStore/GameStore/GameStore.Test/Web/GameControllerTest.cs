using System;
using System.Linq;
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
    public class GameControllerTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IStoreServices> service;
        private static GameController controller;
        private static Mock<IGameService> gameService;
        private Mock<IGenreService> genreService;
        private Mock<IPlatformTypeService> platformService;
        private Mock<IPublisherService> publisherService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            service = new Mock<IStoreServices>();
            gameService = new Mock<IGameService>();
            service.Setup(s => s.GameService).Returns(gameService.Object);
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            controller = new GameController(service.Object, log.Object);
            Mapper.CreateMap<Genre, GenreViewModel>();
            Mapper.CreateMap<PlatformType, PlatformTypeViewModel>();
            Mapper.CreateMap<Publisher, PublisherViewModel>();
            genreService = new Mock<IGenreService>();
            platformService = new Mock<IPlatformTypeService>();
            publisherService = new Mock<IPublisherService>();
            genreService.Setup(s => s.GetAllGenres()).Returns(Enumerable.Empty<Genre>());
            platformService.Setup(s => s.GetAllPlatformTypes()).Returns(Enumerable.Empty<PlatformType>());
            publisherService.Setup(s => s.GetAllPublishers()).Returns(Enumerable.Empty<Publisher>());
            service.Setup(s => s.GenreService).Returns(genreService.Object);
            service.Setup(s => s.PlatformTypeService).Returns(platformService.Object);
            service.Setup(s => s.PublisherService).Returns(publisherService.Object);
            Mapper.CreateMap<Game, GameCreateViewModel>();
            Mapper.CreateMap<Game, GameViewModel>();
            Mapper.CreateMap<GameCreateViewModel, Game>();
            Mapper.CreateMap<GameFilterViewModel, GameFilterModel>();
            Mapper.CreateMap<FilterResult, FilterResultViewModel>();
        }

        [TestMethod]
        public void Call_index_method()
        {
            //Arrange
            var games = new[] {new Game {Key = "Key"}};
            var m = Mapper.Map<GameViewModel>(games[0]);
            gameService.Setup(g => g.GetAllGames()).Returns(games);

            //Act
            var view = controller.Index() as ViewResult;

            //Assert
            Assert.AreEqual(typeof (GameFilterViewModel), view.Model.GetType());
        }

        [TestMethod]
        public void Start_create_new_game()
        {
            //Arrange

            //Act
            var view = controller.New() as ViewResult;

            //Assert
            Assert.IsNotNull(view);
            genreService.Verify(g=>g.GetAllGenres(),Times.Once());
            platformService.Verify(p => p.GetAllPlatformTypes(), Times.Once());
            publisherService.Verify(p => p.GetAllPublishers(), Times.Once());
        }

        [TestMethod]
        public void Create_new_game()
        {
            //Arrange
            var game = new GameCreateViewModel();
            controller.ModelState.Clear();
            gameService.Reset();

            //Act
            var json = controller.New(game) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Get", json.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_not_valid_game()
        {
            //Arrange
            var game = new GameCreateViewModel {Id = 1};
            controller.ModelState.AddModelError("key", "error");

            //Act
            var json = controller.New(game) as ViewResult;

            //Assert
            Assert.AreEqual(game.Id, ((GameCreateViewModel) json.Model).Id);
        }

        [TestMethod]
        public void Create_game_with_ununique_key()
        {
            //Arrange
            var game = new GameCreateViewModel();
            gameService.Setup(g => g.CreateGame(It.IsAny<Game>()))
                .Throws(new ArgumentException("1", "1"));
            controller.ModelState.Clear();

            //Act
            var json = controller.New(game) as ViewResult;

            //Assert
            Assert.AreEqual(game.Id, ((GameCreateViewModel) json.Model).Id);
            Assert.AreEqual(1, controller.ModelState.Count);
        }

        [TestMethod]
        public void Get_game_by_key()
        {
            //Arrange
            var game = new Game {Key = "1"};
            gameService.Setup(g => g.GetGame(game.Key)).Returns(game);

            //Act
            var json = controller.Get(game.Key) as ViewResult;

            //Assert
            Assert.AreEqual(typeof (GameViewModel), json.Model.GetType());
            Assert.AreEqual(game.Key, ((GameViewModel) json.Model).Key);
        }

        [TestMethod]
        public void Get_unexisting_game_by_key()
        {
            //Arrange
            var game = new Game {Key = "1"};
            gameService.Setup(g => g.GetGame(game.Key)).Throws(new ArgumentException("1", "1"));

            //Act
            var res = controller.Get(game.Key);

            //Assert
            Assert.AreEqual(typeof (HttpNotFoundResult), res.GetType());
        }

        [TestMethod]
        public void Start_edit_game()
        {
            //Arrange
            var game = new Game {Key = "Key"};
            gameService.Setup(g => g.GetGame(game.Key)).Returns(game);

            //Act
            var res = controller.Update(game.Key) as ViewResult;

            //Assert
            Assert.AreEqual(game.Key, ((GameCreateViewModel) res.Model).Key);
        }

        [TestMethod]
        public void Edit_game()
        {
            //Arrange
            var game = new GameCreateViewModel();
            controller.ModelState.Clear();

            //Act
            var json = controller.Update(game) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Get", json.RouteValues["action"]);
        }

        [TestMethod]
        public void Edit_not_valid_game()
        {
            //Arrange
            var game = new GameCreateViewModel { Id = 1 };
            controller.ModelState.AddModelError("key", "error");

            //Act
            var json = controller.Update(game) as ViewResult;

            //Assert
            Assert.AreEqual(game.Id, ((GameCreateViewModel)json.Model).Id);
        }

        [TestMethod]
        public void Edit_game_with_ununique_key()
        {
            //Arrange
            var game = new GameCreateViewModel();
            gameService.Setup(g => g.EditGame(It.IsAny<Game>()))
                .Throws(new ArgumentException("1", "1"));
            controller.ModelState.Clear();

            //Act
            var json = controller.Update(game) as ViewResult;

            //Assert
            Assert.AreEqual(game.Id, ((GameCreateViewModel)json.Model).Id);
            Assert.AreEqual(1, controller.ModelState.Count);
        }

        [TestMethod]
        public void Delete_game()
        {
            //Arrange
            long id = 1;

            //Act
            var json = controller.Remove(id) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Manage", json.RouteValues["action"]);
            Assert.AreEqual("Account", json.RouteValues["controller"]);
        }

        [TestMethod]
        public void Delete_unexisting_game()
        {
            //Arrange
            long id = 1;
            gameService.Setup(g => g.DeleteGame(id)).Throws(new ArgumentException("1", "1"));

            //Act
            var res = controller.Remove(id);

            //Assert
            Assert.AreEqual(typeof(HttpNotFoundResult), res.GetType());
        }

        [TestMethod]
        public void Download_game_without_file()
        {
            var game = new Game {Key = "key", ContentType = "app/zip"};
            gameService.Setup(g => g.GetGame(game.Key)).Returns(game);

            //Act
            var res = controller.Download(game.Key);

            //Assert
            Assert.AreEqual(typeof(HttpNotFoundResult), res.GetType());
        }

        [TestMethod]
        public void Download_unexisting_game()
        {
            //Arrange
            var key = "";
            gameService.Setup(g => g.GetGame(key)).Throws(new ArgumentException("1", "1"));

            //Act
            var res = controller.Download(key);

            //Assert
            Assert.AreEqual(typeof(HttpNotFoundResult), res.GetType());
        }

        [TestMethod]
        public void Get_count_of_game()
        {
            //Arrange
            var counts = 5;
            gameService.Setup(g => g.CountGames()).Returns(counts);

            //Act
            var res = controller.GetGameCounts() as ContentResult;

            //Assert
            Assert.AreEqual(counts.ToString(), res.Content);
        }

        [TestMethod]
        public void Try_filter_with_not_valid_filter()
        {
            //Arrange
            var filter = new GameFilterViewModel{Name = "name", PageNumber = 1};
            controller.ModelState.AddModelError("Name", "error");
            gameService.Setup(g => g.FilterGames(It.IsAny<GameFilterModel>())).Returns(new FilterResult());

            //Act
            var result = controller.FilterGames(filter) as PartialViewResult;

            //Assert
            Assert.AreEqual(String.Empty, filter.Name);
        }

        [TestMethod]
        public void Try_filter_with_not_valid_pager()
        {
            //Arrange
            var filter = new GameFilterViewModel { Name = "name" };
            gameService.Setup(g => g.FilterGames(It.IsAny<GameFilterModel>())).Returns(new FilterResult());

            //Act
            var result = controller.FilterGames(filter) as PartialViewResult;

            //Assert
            Assert.AreEqual(1, filter.PageNumber);
        }

        [TestMethod]
        public void Try_filter_games()
        {
            //Arrange
            var filter = new GameFilterViewModel();
            gameService.Setup(g => g.FilterGames(It.IsAny<GameFilterModel>())).Returns(new FilterResult());

            //Act
            var result = controller.FilterGames(filter) as PartialViewResult;

            //Assert
            Assert.AreEqual(typeof(FilterResultViewModel), result.Model.GetType());
        }
    }
}
