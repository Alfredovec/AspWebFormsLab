using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
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
    public class UserServiceTest
    {
        private static Mock<ILogger> log;
        private static Mock<IUnitOfWork> unitOfWork;
        private static Mock<IUserRepository> repo;
        private static IUserService userService;

        [ClassInitialize]
        public static void ClassInitiallizer(TestContext context)
        {
            log = new Mock<ILogger>();
            unitOfWork = new Mock<IUnitOfWork>();
            userService = new StoreService(unitOfWork.Object, log.Object).UserService;
        }

        [TestInitialize]
        public void TestInitiallizer()
        {
            unitOfWork.ResetCalls();
            repo = new Mock<IUserRepository>();
            unitOfWork.SetupGet(u => u.UserRepository).Returns(repo.Object);
        }

        [TestMethod]
        public void Ban_user()
        {
            //Arrange
            var user = new User{Email = "email"};
            var period = new TimeSpan();

            //Act
            userService.Ban(user, period);

            //Assert
            repo.Verify(r => r.BanUser(user.Email, period), Times.Once);
        }

        [TestMethod]
        public void Get_user_or_or_default()
        {
            //Arrange
            var user = "user";

            //Act
            userService.GetUserOrDefault(user);

            //Assert
            repo.Verify(r => r.GetUser(user), Times.Once);
        }

        [TestMethod]
        public void Correct_login_user()
        {
            //Arrange
            var user = new User
            {
                Email = "login",
                Password = "password"
            };
            repo.Setup(r => r.LoginUser(user.Email, user.Password)).Returns(user);

            //Act
            var res = userService.LoginUser(user.Email, user.Password);

            //Assert
            Assert.AreEqual(user, res);
        }

        [TestMethod]
        public void Get_user_with_invalid_login()
        {
            //Arrange
            User user = null;
            var email = "email";
            var password = "pass";

            repo.Setup(r => r.LoginUser("","")).Returns(user);

            //Act
            var res = userService.LoginUser(email, password);

            //Assert
            Assert.IsNull(res);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void Login_user_with_exception()
        {
            //Arrange
            var user = new User
            {
                Email = "login",
                Password = "password"
            };
            repo.Setup(r => r.LoginUser(user.Email, user.Password)).Throws(new AuthenticationException());

            //Act
            userService.LoginUser(user.Email, user.Password);

            //Assert
        }
        
        [TestMethod]
        public void Register_user()
        {
            //Arrange
            var user = new User();

            //Act
            userService.RegisterUser(user);

            //Assert
            repo.Verify(r => r.Create(user), Times.Once);
        }

        [TestMethod]
        public void Get_role()
        {
            //Arrange
            var roleName = "role";

            //Act
            userService.GetRole(roleName);

            //Assert
            repo.Verify(r => r.GetRole(roleName), Times.Once);
        }

        [TestMethod]
        public void Get_all_users()
        {
            //Arrange

            //Act
            userService.GetUsers();

            //Assert
            repo.Verify(r => r.Get(), Times.Once());
        }

        [TestMethod]
        public void Get_role_by_id()
        {
            //Arrange
            var roleId = 1;
            
            //Act
            userService.GetRole(roleId);

            //Assert
            repo.Verify(r => r.GetRole(roleId), Times.Once());
        }

        [TestMethod]
        public void Get_roles()
        {
            //Arrange

            //Act
            userService.GetRoles();

            //Assert
            repo.Verify(r => r.GetRoles(), Times.Once());
        }

        [TestMethod]
        public void Get_user()
        {
            //Arrange
            var userId = 1;

            //Act
            userService.GetUser(userId);

            //Assert
            repo.Verify(r => r.Get(userId), Times.Once());
        }

        [TestMethod]
        public void Update_user()
        {
            //Arrange
            var user = new User();

            //Act
            userService.UpdateUser(user);

            //Assert
            repo.Verify(r => r.Edit(user), Times.Once());
            unitOfWork.Verify(u=>u.Save(),Times.Once());
        }

        [TestMethod]
        public void Delete_user()
        {
            //Arrange
            var user = new User {Id = 1};
            repo.Setup(r => r.Get(user.Id)).Returns(user);

            //Act
            userService.DeleteUser(user.Id);

            //Assert
            repo.Verify(r => r.Delete(user), Times.Once());
            unitOfWork.Verify(u => u.Save(), Times.Once());
        }

        [TestMethod]
        public void Is_banned_with_null_activation_time()
        {
            //Arrange
            var user = new User{Email = "email"};
            repo.Setup(r=>r.GetUser(user.Email)).Returns(user);

            //Act
            var res = userService.IsBanned(user);

            //Assert
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void Is_banned_with_activation_time()
        {
            //Arrange
            var user = new User { Email = "email", ActivationTime = DateTime.UtcNow.AddDays(1)};
            repo.Setup(r => r.GetUser(user.Email)).Returns(user);

            //Act
            var res = userService.IsBanned(user);

            //Assert
            Assert.IsTrue(res);
        }
    }
}
