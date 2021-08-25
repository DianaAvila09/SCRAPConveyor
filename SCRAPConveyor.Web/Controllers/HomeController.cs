using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCRAPConveyor.Web.Controllers
{
    public class HomeController : Controller
    {
        Security.cls_Security Security = new Security.cls_Security();
        public ActionResult Index()
        {
            //string usr = Security.Encripta("User=userBTS;Password=userBTS_01");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}