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
    public class UserRepositoryTest
    {
        private static IUserRepository repository;
        private static Mock<GameStoreContext> dbContext;
        private static IDbSet<User> data;

        [TestInitialize]
        public void TestInitiallizer()
        {
            dbContext = new Mock<GameStoreContext>();
            repository = new GameStoreUserRepository(dbContext.Object);
            data = new FakeDbSet<User>();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new User {
                    Id = i, 
                    Roles = new List<Role>(), 
                    Email = "Name" + i, 
                    Password = "Pass" + i
                });
            }
            dbContext.SetupGet(c => c.Users).Returns(data);
        }

        [TestMethod]
        public void Create_user()
        {
            //Arrange
            var userId = 100;
            var type = new User { Id = userId };

            //Act
            repository.Create(type);

            //Assert
            Assert.AreEqual(11, data.Count());
            Assert.AreEqual(type, data.First(p => p.Id == userId));
        }

        [TestMethod]
        public void Edit_user()
        {
            //Arrange
            var userId = 1;
            var type = new User { Id = userId, Name = "newName"};

            //Act
            repository.Edit(type);

            //Assert
            Assert.AreEqual(type.Name, data.First(u=>u.Id==userId).Name);

        }

        [TestMethod]
        public void Delete_user()
        {
            //Arrange
            var userId = 1;
            var type = data.First(u => u.Id == userId);

            //Act
            repository.Delete(type);

            //Assert
            Assert.AreEqual(9, data.Count());
            Assert.IsTrue(data.All(p => p.Id != userId));
        }

        [TestMethod]
        public void Get_user_by_id()
        {
            //Arrange
            var userId = 1;
            var type = data.First(u => u.Id == userId);

            //Act
            var res = repository.Get(userId);

            //Assert
            Assert.AreEqual(type, res);
        }

        [TestMethod]
        public void Get_all_users()
        {
            //Arrange

            //Act
            var res = repository.Get();

            //Assert
            Assert.AreEqual(10, res.Count());
        }

        [TestMethod]
        public void Get_users_with_predicate()
        {
            //Arrange
            var userId = 1;
            Func<User, bool> predicate = p => p.Id == userId;
            var type = data.First(predicate);

            //Act
            var res = repository.Get(predicate);

            //Assert
            Assert.AreEqual(type, res.First());
        }

        [TestMethod]
        public void Get_user_by_login()
        {
            //Arrange
            var login = "Name2";
            var type = data.First(u => u.Email == login);

            //Act
            var res = repository.GetUser(login);

            //Assert
            Assert.AreEqual(type, res);
        }

        [TestMethod]
        public void Get_user_by_login_and_password()
        {
            //Arrange
            var login = "Name2";
            var type = data.First(u => u.Email == login);

            //Act
            var res = repository.LoginUser(login, type.Password);

            //Assert
            Assert.AreEqual(type, res);
        }
    }
}
