using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Authorization.Infrastructure;
using GameStore.Authorization.Interfaces;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthorization _authorization;

        public AccountController(IAuthorization authorization, IStoreServices service)
            : base(service)
        {
            _authorization = authorization;
        }

        public ActionResult Login()
        {
            var user = new UserViewModel
            {
                RedirectUrl = Request.UrlReferrer == null ? "/" : Request.UrlReferrer.AbsolutePath
            };
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var loginedUser = _authorization.Login(user.Email, user.Password, user.IsPersistent);
            if (loginedUser == null)
            {
                ModelState.AddModelError("", GlobalRes.Login_or_password_is_incorrect_or_users_is_banned);
                return View(user);
            }
            return Redirect(user.RedirectUrl);
        }

        public ActionResult Logout()
        {
            _authorization.LogOut();
            return RedirectToAction("Index", "Game");
        }

        public ActionResult Register()
        {
            var user = new RegisterUserViewModel
            {
                RedirectUrl = Request.UrlReferrer == null ? "/" : Request.UrlReferrer.AbsolutePath
            };
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(RegisterUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            _authorization.RegisterUser(Mapper.Map<User>(user));
            return RedirectToAction("Index", "Game");
        }

        [LocalizableAuthorize(Roles = "Administrator,Manager")]
        public ActionResult Manage()
        {
            return View();
        }

        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult Edit(long id)
        {
            var user = Mapper.Map<UserCreateViewModel>(_storeServices.UserService.GetUser(id));
            user.Password = string.Empty;
            SetAllRolesToUser(user);
            return View(user);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult Edit(UserCreateViewModel user)
        {
            if (ModelState.IsValid)
            {
                _storeServices.UserService.UpdateUser(Mapper.Map<User>(user));
                return RedirectToAction("Manage");
            }
            SetAllRolesToUser(user);
            return View(user);
        }

        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult Users()
        {
            var users = _storeServices.UserService.GetUsers().Select(Mapper.Map<UserDetailsViewModel>);
            return View(users);
        }

        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult CreateUser()
        {
            var user = new UserCreateViewModel();
            SetAllRolesToUser(user);
            return View(user);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult CreateUser(UserCreateViewModel user)
        {
            if (ModelState.IsValid)
            {
                _storeServices.UserService.RegisterUser(Mapper.Map<User>(user));
                return RedirectToAction("Manage");
            }
            SetAllRolesToUser(user);
            return View(user);
        }
        
        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult Delete(long id)
        {
            var user = Mapper.Map<UserDetailsViewModel>(_storeServices.UserService.GetUser(id));
            return View(user);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult Delete(long id, FormCollection form)
        {
            _storeServices.UserService.DeleteUser(id);
            return RedirectToAction("Users");
        }

        [LocalizableAuthorize(Roles = "Administrator")]
        public ActionResult Details(long id)
        {
            var user = Mapper.Map<UserDetailsViewModel>(_storeServices.UserService.GetUser(id));
            return View(user);
        }

        private void SetAllRolesToUser(UserCreateViewModel user)
        {
            user.Roles = _storeServices.UserService.GetRoles().Select(Mapper.Map<RoleViewModel>).ToList();
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult ManageNotificationStatus()
        {
            var profile = Mapper.Map<ManagerProfileViewModel>(User.Identity.GetUser().ManagerProfile??new ManagerProfile());
            return View(profile);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult ManageNotificationStatus(ManagerProfileViewModel profile)
        {
            if (ModelState.IsValid)
            {
                _storeServices.UserService.ChangeNotificationManagerStatus(User.Identity.GetUser().Id, Mapper.Map<ManagerProfile>(profile));
                return RedirectToAction("Manage");
            }
            return View(profile);
        }
    }
}
