using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    public class PublisherController : BaseController
    {
        private readonly ILogger _logger;

        public PublisherController(IStoreServices storeServices, ILogger logger) : base(storeServices)
        {
            _logger = logger;
        }

        public ActionResult Details(string companyName)
        {
            try
            {
                var publisher = _storeServices.PublisherService.GetPublisher(companyName);
                return View(Mapper.Map<PublisherViewModel>(publisher));
            }
            catch (ArgumentException)
            {
                return new HttpNotFoundResult("This comapne not found");
            }
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult All()
        {
            var publishers = _storeServices.PublisherService.GetAllPublishers().Select(Mapper.Map<PublisherViewModel>).ToList();
            return View(publishers);
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Edit(long id)
        {
            var publisher = Mapper.Map<PublisherCreateViewModel>(_storeServices.PublisherService.GetPublisher(id));
            return View(publisher);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Edit(PublisherCreateViewModel publisher)
        {
            if (ModelState.IsValid)
            {
                _storeServices.PublisherService.EditPublisher(Mapper.Map<Publisher>(publisher));
                return RedirectToAction("Manage", "Account");
            }
            return View(publisher);
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult New(PublisherCreateViewModel publisher)
        {
            if (!ModelState.IsValid)
            {
                return View(publisher);
            }
            try
            {
                _storeServices.PublisherService.CreatePublisher(Mapper.Map<Publisher>(publisher));
                return RedirectToAction("Details", new {companyName = publisher.CompanyName});
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError(e.ParamName, "Company name must be unique");
                return View(publisher);
            }   
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Delete(long id)
        {
            var publisher = Mapper.Map<PublisherViewModel>(_storeServices.PublisherService.GetPublisher(id));
            return View(publisher);
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Delete(long id, FormCollection form)
        {
            _storeServices.PublisherService.RemovePublisher(id);
            return RedirectToAction("Manage", "Account");
        }
    }
}
