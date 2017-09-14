using JsonWebClientTestserver.Models;
using System;
using System.Web.Mvc;

namespace JsonWebClientTestserver.Controllers
{
    public class DownloadCheckController : TestResultController
    {
        [HttpPost]
        public JsonResult DownloadOneAbc()
        {
            return Json(new Abc() { a = 1, b =new Bclass(1), c = "1" });
        }

        [HttpPatch]
        public JsonResult DownloadServerDateTime()
        {
            return Json(DateTime.Now);
        }
    }
}