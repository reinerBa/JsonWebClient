using System.Web.Mvc;
using JsonWebClientTestserver.Models;
using bamberger.rocks;
using System.Collections.Generic;
using System;

// link for proper test framework https://www.strathweb.com/2015/05/integration-testing-asp-net-5-asp-net-mvc-6-applications/
namespace JsonWebClientTestserver.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Url.AbsoluteUri.Contains(@"Home/Index"))
                return Redirect("/");
            ViewBag.Title = "Home Page";
            var model = new IndexViewModel();
            var abc = new Abc { a = 1, b = new Bclass(2.2), c = "Drei" };

            using (JsonWebClient client = new JsonWebClient())
                model.results.Add("Result1", 
                    client.UploadObject(url(@"Home/EchoAOfABC"), abc));
            
            using (JsonWebClient client = new JsonWebClient())
            {
                Cba r = client.UpDownloadObject<Cba>(url( @"Home/EchoAbcAsCba"), abc);
                model.results.Add("Result2",
                    $"Result has type {r.GetType()} and values {r}");
            }

            // Download
            model.results.Add("Download one Abc:", 
                JsonWebClient.DownloadObject<Abc>(url(@"DownloadCheck/DownloadOneAbc")).ToString());
            model.results.Add("Download a DateTime:",
                JsonWebClient.DownloadObject<DateTime>(url(@"DownloadCheck/DownloadServerDateTime"), "PATCH").ToString("u"));


            //UpDownload
            var abc3 = new Abc() { a = 11, b = new Bclass(2.2), c = "Drei und Dreißig" };
            var cba3 = new Cba() { a = 333, b = new Bclass(2.22), c = "Hundertelf" };
            var transportList = new List<object>();
            transportList.Add(abc3);
            transportList.Add(cba3);

            model.results.Add("UpDownload two objects, the secound is:",
                JsonWebClient.UpDownloadObject<Cba>(transportList, url(@"UpDownloadCheck/UpDownloadOneCbaFromAbc")).ToString());


            return View(model);
        }
        private string url(string suffix)
        {
            return this.Request.Url.AbsoluteUri + suffix;
        }

        [AcceptVerbs("GET", "POST")]
        public ActionResult EchoAOfAbc(Abc abc, string info)
        {
            return Content("A of Abc is :" + abc?.a);
        }

        [AcceptVerbs("GET", "POST")]
        public JsonResult EchoAbcAsCba(Abc abc)
        {
            if (abc == null)
                return Json(null);
            var cba = new Cba() { a = abc.a, b = abc.b, c = abc.c };

            return Json(cba, JsonRequestBehavior.AllowGet);
        }
    }
}
