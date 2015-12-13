using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Models.Services;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    public class OrdersController : BaseController
    {
        public OrdersController(IStoreServices storeServices) : base(storeServices) { }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Index()
        {
            var diaposon = new DateDiaposonViewModel
            {
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            return View(diaposon);
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult History()
        {
            var diaposon = new DateDiaposonViewModel
            {
                EndDate = DateTime.UtcNow.AddMonths(-1)
            };
            return View(diaposon);
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult FindOrdersForEditing(DateDiaposonViewModel model)
        {
            if (ModelState.IsValid)
            {
                return PartialView(_storeServices.OrderService.GetHistory(model.StartDate, model.EndDate).Select(Mapper.Map<OrderViewModel>));
            }
            return HttpNotFound("Model is not valid");
        }

        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult FindOrders(DateDiaposonViewModel model)
        {
            if (ModelState.IsValid)
            {
                return PartialView(_storeServices.OrderService.GetHistory(model.StartDate, model.EndDate).Select(Mapper.Map<OrderViewModel>));
            }
            return HttpNotFound("Model is not valid");
        }

        [HttpPost]
        [LocalizableAuthorize(Roles = "Manager")]
        public ActionResult Edit(long id)
        {
            _storeServices.OrderService.ShippOrder(id);
            return RedirectToAction("Index");
        }
    }
}
