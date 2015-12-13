using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using GameStore.Authorization.Interfaces;
using GameStore.Models.Entities;
using GameStore.Models.Services;

namespace GameStore.Authorization
{
    public class GameStoreAuthorization : IAuthorization
    {
        private IPrincipal _currentUser;

        public HttpContext HttpContext { get; set; }

        private readonly IUserService _service;

        public GameStoreAuthorization(IStoreServices services)
        {
            _service = services.UserService;
        }

        public User Login(string userName, string password, bool isPersistent)
        {
            var retUser = _service.LoginUser(userName, password);
            if (retUser != null)
                CreateCookie(userName, isPersistent);
            return retUser;
        }

        public void RegisterUser(User user)
        {
            _service.RegisterUser(user);
            Login(user.Name, user.Password, false);
        }

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                    1, userName, DateTime.UtcNow,
                    DateTime.UtcNow.Add(FormsAuthentication.Timeout),
                    isPersistent, string.Empty,
                    FormsAuthentication.FormsCookiePath);

            var encruptTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                Value = encruptTicket,
                Expires = DateTime.UtcNow.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authCookie);
        }


        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie != null)
                httpCookie.Value = string.Empty;
        }

        public void Ban(User user, TimeSpan period)
        {
            _service.Ban(user, period);
        }

        public User GetUserOrDefault(string name)
        {
            return _service.GetUserOrDefault(name);
        }

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        var authCookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(ticket.Name, _service);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception)
                    {
                        _currentUser = new UserProvider(null, null);
                    }
                }
                return _currentUser;
            }
        }
    }
}
