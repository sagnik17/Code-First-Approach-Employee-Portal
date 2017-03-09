using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeePortal_Assignment.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Start()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}