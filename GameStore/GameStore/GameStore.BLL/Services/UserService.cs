using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;

namespace GameStore.BLL.Services
{
    class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        public UserService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Ban(User user, TimeSpan period)
        {
            using (_logger.LogPerfomance())
            {
                _unitOfWork.UserRepository.BanUser(user.Email, period);
            }
        }

        public User GetUserOrDefault(string name)
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.UserRepository.GetUser(name);
            }
        }

        public User LoginUser(string name, string password)
        {
            using (_logger.LogPerfomance())
            {
                var user = _unitOfWork.UserRepository.LoginUser(name, password);
                if (user == null)
                {
                    _logger.LogInfo(
                        string.Format("Somebody try login with incorrect data: [Login: {0}, Password:{1}]",
                            name, password));
                }
                else
                {
                    _logger.LogInfo(
                        string.Format("User {0} succes logined at {1}",
                            name, DateTime.UtcNow));
                }
                return user;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            return _unitOfWork.UserRepository.Get();
        }

        public void RegisterUser(User user)
        {
            _unitOfWork.UserRepository.Create(user);
            _unitOfWork.Save();
        }

        public Role GetRole(string roleName)
        {
            return _unitOfWork.UserRepository.GetRole(roleName);
        }

        public Role GetRole(long roleId)
        {
            return _unitOfWork.UserRepository.GetRole(roleId);
        }

        public IEnumerable<Role> GetRoles()
        {
            return _unitOfWork.UserRepository.GetRoles();
        }

        public User GetUser(long id)
        {
            return _unitOfWork.UserRepository.Get(id);
        }

        public void UpdateUser(User user)
        {
            _unitOfWork.UserRepository.Edit(user);
            _unitOfWork.Save();
        }

        public void DeleteUser(long id)
        {
            _unitOfWork.UserRepository.Delete(_unitOfWork.UserRepository.Get(id));
            _unitOfWork.Save();
        }

        public bool IsBanned(User user)
        {
            var originUser = _unitOfWork.UserRepository.GetUser(user.Email);
            if (originUser.ActivationTime == null)
            {
                return false;
            }
            return originUser.ActivationTime > DateTime.UtcNow;
        }

        public void ChangeNotificationManagerStatus(long userId, ManagerProfile managerProfile)
        {
            using (_logger.LogPerfomance())
            {
                var user = _unitOfWork.UserRepository.Get(userId);
                managerProfile.User = user;
                _unitOfWork.UserRepository.UpdateManagerProfile(managerProfile);
                _unitOfWork.Save();
            }
        }
    }
}