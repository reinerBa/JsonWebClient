using JsonWebClientTestserver.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JsonWebClientTestserver.Controllers
{
    public class UpDownloadCheckController : TestResultController
    {
        [HttpPost]
        public JsonResult UpDownloadOneAbc(Abc abc)
        {
            this.uploadedObject = abc;
            this.nullsInInputs = abc.a == null || abc.b == null || abc.c == null;
            return Json(new Abc() { a = abc.a, b = abc.b, c = abc.c });
        }

        [HttpPost]
        public JsonResult UpDownloadOneCbaFromAbc(Abc abc, Cba cba)
        {
            this.uploadedObject = new List<object> { abc, cba };
            this.nullsInInputs = abc.a == null || abc.b == null || abc.c == null || cba.a == null || cba.b == null || cba.c == null;
            return Json(new Cba() { a = abc.a, b = abc.b, c = abc.c });
        }
    }
}