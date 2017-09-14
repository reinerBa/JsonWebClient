using JsonWebClientTestserver.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JsonWebClientTestserver.Controllers
{
    public class UploadCheckController : TestResultController
    {
        [HttpPost]
        public ActionResult UploadOneAbc(Abc abc)
        {
            this.uploadedObject = abc;
            this.nullsInInputs = abc.a == null || abc.b == null || abc.c == null;
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UploadOneAbc(Abc abc, Cba cba)
        {
            this.uploadedObject = new List<object> {abc, cba};
            this.nullsInInputs = abc.a == null || abc.b == null || abc.c == null || cba.a == null || cba.b == null || cba.c == null;
            return new EmptyResult();
        }
    }
}