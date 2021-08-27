using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCRAPConveyor.Web.Controllers
{
    public class HomeController : Controller
    {
        
        Operacion.cls_Operacion Operacion = new Operacion.cls_Operacion();
        public ActionResult Index()
        {            
            //Security.cls_Security Security = new Security.cls_Security();
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