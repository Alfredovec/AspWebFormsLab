using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GameStore.Models.Entities;

namespace GameStore.Authorization.Interfaces
{
    public interface IAuthorization
    {
        HttpContext HttpContext { get; set; }

        User Login(string login, string password, bool isPersistent);

        void RegisterUser(User user);

        void LogOut();

        void Ban(User user, TimeSpan period);

        User GetUserOrDefault(string name);

        IPrincipal CurrentUser { get; }
    }

}
