using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Models.Services;
using GameStore.Web.Filters;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    [LocalizableAuthorize(Roles = "Manager")]
    public class GenreController : BaseController
    {
        public GenreController(IStoreServices service) : base(service) { }

        public ActionResult Index()
        {
            return Content("Genres");
        }

        public ActionResult Create()
        {
            return Content("Create");
        }

        public ActionResult Edit(long id)
        {
            return Content("Edit " + id);
        }

        public ActionResult Delete(long id)
        {
            return Content("Delete " + id);
        }
    }
}
