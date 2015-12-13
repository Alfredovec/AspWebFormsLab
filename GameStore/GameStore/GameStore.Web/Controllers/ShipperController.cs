using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Models.Services;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    public class ShipperController : BaseController
    {
        public ShipperController(IStoreServices services) : base(services) { }

        public ActionResult Index()
        {
            return Json(_storeServices.ShipperService.GetAll().Select(Mapper.Map<ShipperViewModel>).ToList(),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(long id)
        {
            return Json(Mapper.Map<ShipperViewModel>(_storeServices.ShipperService.Get(id)),
                JsonRequestBehavior.AllowGet);
        }

    }
}
