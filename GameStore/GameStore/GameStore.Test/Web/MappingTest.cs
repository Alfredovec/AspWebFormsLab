using AutoMapper;
using GameStore.Models.Services;
using GameStore.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Test.Web
{
    [TestClass]
    public class MappingTest
    {
        [TestMethod]
        public void Test_All_Mapps()
        {
            //Arrange
            var service = new Mock<IStoreServices>();
            var mapper = new GameStoreMappers(service.Object);
            
            //Act
            mapper.RegiterAllMaps();

            //Assert
            Assert.IsTrue(Mapper.GetAllTypeMaps().Length>0);
        }
    }
}
