using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;

namespace GameStore.Models.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(string login);

        void AddRole(string login, Role role);

        User LoginUser(string login, string password);

        void BanUser(string login, TimeSpan period);

        Role GetRole(string roleName);

        IEnumerable<Role> GetRoles();

        Role GetRole(long roleId);

        void UpdateManagerProfile(ManagerProfile managerProfile);
    }
}
