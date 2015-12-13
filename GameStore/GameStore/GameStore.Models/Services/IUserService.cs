using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface IUserService
    {
        void Ban(User user, TimeSpan period);

        User GetUserOrDefault(string name);

        User LoginUser(string name, string password);

        IEnumerable<User> GetUsers(); 

        void RegisterUser(User user);

        Role GetRole(string roleName);

        Role GetRole(long roleId);

        IEnumerable<Role> GetRoles();
        
        User GetUser(long id);
         
        void UpdateUser(User user);
        
        void DeleteUser(long id);

        bool IsBanned(User user);

        void ChangeNotificationManagerStatus(long userId, ManagerProfile managerProfile);
    }
}
