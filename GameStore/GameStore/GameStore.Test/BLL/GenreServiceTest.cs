using System;
using GameStore.BLL.Services;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.BLL
{
    [TestClass]
    public class GenreServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IGenreRepository> repo;
        private static IStoreServices storeServices;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            storeServices = new StoreService(unitOfWork.Object, log.Object);
            repo = new Mock<IGenreRepository>();
        }


        [TestMethod]
        public void Try_get_genre_by_id()
        {
            //Arrange
            var genreService = storeServices.GenreService;
            unitOfWork.SetupGet(u => u.GenreRepository).Returns(repo.Object);

            //Act
            var resultGenre = genreService.GetGenre(1);

            //Assert
            repo.Verify(g => g.Get(It.IsAny<long>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Try_get_unexesting_genre_by_id()
        {
            //Arrange
            var genreService = storeServices.GenreService;
            unitOfWork.SetupGet(u => u.GenreRepository).Returns(repo.Object);
            repo.Setup(r => r.Get(1)).Throws<InvalidOperationException>();

            //Act
            var resultPlatform = genreService.GetGenre(1);

            //Assert
        }

        [TestMethod]
        public void Try_get_genres_by_game_id()
        {
            var genreService = storeServices.GenreService;
            unitOfWork.SetupGet(u => u.GenreRepository).Returns(repo.Object);

            //Act
            var resultGenres = genreService.GetGenresForGame(1);

            //Assert
            repo.Verify(g => g.GetGenresByGameId(It.IsAny<long>()));
        }

        [TestMethod]
        public void Try_get_all_genres()
        {
            //Arrange
            var genreService = storeServices.GenreService;
            unitOfWork.SetupGet(u => u.GenreRepository).Returns(repo.Object);

            //Act
            genreService.GetAllGenres();

            //Assert
            repo.Verify(p => p.Get(), Times.Once());
        }
    }
}
