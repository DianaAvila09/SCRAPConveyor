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

        List<Operacion.cls_Operacion.TrailerInformation> Resultado = new List<Operacion.cls_Operacion.TrailerInformation>();
        

        public ActionResult Index()
        {
            
            //Security.cls_Security Security = new Security.cls_Security();
            //string claveEncriptada = Security.Encripta("User=userBTS;Password=userBTS_01");            
            return View(Resultado);
        }

        [HttpGet]        
        public JsonResult Views()
        {
            Resultado = Operacion.get_TrailerInformation();

            var respuesta = Json(Resultado, JsonRequestBehavior.AllowGet);
            return (respuesta);
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