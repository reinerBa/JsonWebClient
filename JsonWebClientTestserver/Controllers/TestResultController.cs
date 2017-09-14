using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsonWebClientTestserver.Controllers
{
    public abstract class TestResultController : Controller
    {
        public object uploadedObject;
        public bool nullsInInputs;
    }
}