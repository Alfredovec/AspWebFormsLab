using System.Collections.Generic;
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
    public class TranslateServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static ITranslateService translateService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            translateService = new StoreService(unitOfWork.Object, log.Object).TranslateService;
        }

        [TestMethod]
        public void Get_descrition_for_game()
        {
            //Arrange
            var desc = "descrition";
            var language = "en";
            var translates = new List<GameTranslation>
            {
                new GameTranslation {Language = Language.En, Description = desc}
            };

            //Act
            var res = translateService.GetGameDescription(language, translates);

            //Assert
            Assert.AreEqual(desc, res);
        }

        [TestMethod]
        public void Get_descrition_for_game_by_default()
        {
            //Arrange
            var desc = "descrition";
            var language = "en";
            var translates = new List<GameTranslation>
            {
                new GameTranslation {Language = GameTranslation.DefaultLanguage, Description = desc}
            };

            //Act
            var res = translateService.GetGameDescription(language, translates);

            //Assert
            Assert.AreEqual(desc, res);
        }

        [TestMethod]
        public void Get_name_for_genre()
        {
            //Arrange
            var desc = "descrition";
            var language = "en";
            var translates = new List<GenreTranslation>
            {
                new GenreTranslation {Language = Language.En, Name = desc}
            };

            //Act
            var res = translateService.GetGenreName(language, translates);

            //Assert
            Assert.AreEqual(desc, res);
        }

        [TestMethod]
        public void Get_name_for_genre_by_default()
        {
            //Arrange
            var desc = "descrition";
            var language = "en";
            var translates = new List<GenreTranslation>
            {
                new GenreTranslation {Language = GameTranslation.DefaultLanguage, Name = desc}
            };

            //Act
            var res = translateService.GetGenreName(language, translates);

            //Assert
            Assert.AreEqual(desc, res);
        }

        [TestMethod]
        public void Get_descrition_for_publisher()
        {
            //Arrange
            var desc = "descrition";
            var language = "en";
            var translates = new List<PublisherTranslation>
            {
                new PublisherTranslation {Language = Language.En, Description = desc}
            };

            //Act
            var res = translateService.GetPublisherDescrition(language, translates);

            //Assert
            Assert.AreEqual(desc, res);
        }

        [TestMethod]
        public void Get_descrition_for_publisher_by_default()
        {
            //Arrange
            var desc = "descrition";
            var language = "en";
            var translates = new List<PublisherTranslation>
            {
                new PublisherTranslation {Language = GameTranslation.DefaultLanguage, Description = desc}
            };

            //Act
            var res = translateService.GetPublisherDescrition(language, translates);

            //Assert
            Assert.AreEqual(desc, res);
        }
    }
}
