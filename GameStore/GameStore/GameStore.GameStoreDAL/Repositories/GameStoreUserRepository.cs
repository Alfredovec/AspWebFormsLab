using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    class GameStoreUserRepository : IUserRepository
    {
        private readonly GameStoreContext _db;

        public GameStoreUserRepository(GameStoreContext db)
        {
            _db = db;
        }

        public void Create(User item)
        {
            item.ActivationTime = DateTime.UtcNow;
            _db.Users.Add(item);
        }

        public void Edit(User item)
        {
            var old = Get(item.Id);
            old.Name = item.Name;
            old.Password = item.Password;
            old.Roles.Clear();
            old.Roles = item.Roles;
        }

        public void Delete(User item)
        {
            _db.Users.Remove(item);
        }

        public User Get(long id)
        {
            return _db.Users.Include(u => u.Roles).First(u => u.Id == id);
        }

        public IEnumerable<User> Get()
        {
            return _db.Users.Include(u => u.Roles).ToList();
        }

        public IEnumerable<User> Get(Func<User, bool> predicate)
        {
            return _db.Users.Include(u => u.Roles).Where(predicate).ToList();
        }

        public User GetUser(string name)
        {
            return _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Email == name);
        }

        public void AddRole(string name, Role role)
        {
            var user = GetUser(name);
            user.Roles.Add(role);
            _db.Entry(user).State = EntityState.Modified;
        }

        public User LoginUser(string name, string password)
        {
            return _db.Users.FirstOrDefault(u => u.Email == name && u.Password == password);
        }

        public void BanUser(string name, TimeSpan period)
        {
            var user = GetUser(name);
            user.ActivationTime = DateTime.UtcNow.Add(period);
            _db.Entry(user).State = EntityState.Modified;
        }

        public Role GetRole(string roleName)
        {
            return _db.Roles.First(r => r.Name == roleName);
        }

        public IEnumerable<Role> GetRoles()
        {
            return _db.Roles.ToList();
        }

        public Role GetRole(long roleId)
        {
            return _db.Roles.Find(roleId);
        }

        public void UpdateManagerProfile(ManagerProfile managerProfile)
        {
            var profileOrigin = _db.ManagerProfiles.FirstOrDefault(p => p.User.Id == managerProfile.User.Id);
            if (profileOrigin==null)
            {
                _db.ManagerProfiles.Add(managerProfile);
                return;
            }
            profileOrigin.NotifyStatus = managerProfile.NotifyStatus;
            _db.Entry(profileOrigin).State = EntityState.Modified;
        }
    }
}