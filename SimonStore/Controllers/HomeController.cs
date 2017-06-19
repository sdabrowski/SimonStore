using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.Controllers
{
    public class HomeController : Controller
    {
        //[CartCalculator]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to Simon's Paracord Store!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Have a question or concern? Let us know.";

            return View();
        }
    }
}