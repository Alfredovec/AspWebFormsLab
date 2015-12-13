using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GameStore.Models.Services;

namespace GameStore.Web.Infrastructure.HttpHandlers
{
    public class PictureAsyncHandler : HttpTaskAsyncHandler
    {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            string url = context.Request.Url.Segments.Last();
            var service = DependencyResolver.Current.GetService<IStoreServices>();
            var picture = await Task.Run(() =>service.GameService.GetImage(url.Split('.')[0]));

            context.Response.ContentType = picture.Item2;
            context.Response.BinaryWrite(picture.Item1);
        }

        public override bool IsReusable
        {
            get { return true; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            throw new Exception("The ProcessRequest method has no implementation.");
        }
    }
}
