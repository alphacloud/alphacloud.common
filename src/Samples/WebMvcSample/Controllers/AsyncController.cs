using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvcSample.Controllers
{
    using System.Threading.Tasks;

    public class AsyncController : Controller
    {
        // GET: Async
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> Download()
        {
            var svc = new DownloadService(new Uri("http://az.lib.ru/k/karamzin_n_m/text_0040.shtml"));
            var text = await svc.Download() ?? string.Empty;

            ViewBag.Response = text;
            ViewBag.ResponseLength = text.Length;
            return View();
        }
    }
}