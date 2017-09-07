﻿using JsonWebClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsonWebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var model = new IndexViewModel();
            var abc = new Abc { a = 1, b = 2, c = 3 };

            using (JsonWebClient client = new JsonWebClient())
            {
                model.Result1 = client.UploadObject(this.Request.Url.AbsoluteUri + @"Home/EchoAOfABC", abc);
            }
            using (JsonWebClient client = new JsonWebClient())
            {
                CBA r = client.DownloadObject<CBA>(this.Request.Url.AbsoluteUri + @"Home/EchoAbcAsCba", abc);
                model.Result2 = $"Result has type {r.GetType()} and values {r}";
            }

            return View(model);
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
            var cba = new CBA() { a = abc.a, b = abc.b, c = abc.c };

            return Json(cba, JsonRequestBehavior.AllowGet);
        }
    }
}
