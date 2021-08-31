using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Operacion;
using Historico;


namespace SCRAPConveyor.Web.Controllers
{
    public class HomeController : Controller
    {
        cls_Operacion Operacion = new cls_Operacion();
        cls_Historico Historico = new cls_Historico();

        List<cls_Operacion.TrailerInformation> Resultado = new List<cls_Operacion.TrailerInformation>();
        List<cls_Historico.HistoryTrailerInformation> ResultadoHistorico = new List<cls_Historico.HistoryTrailerInformation>();


        public ActionResult Index()
        {            
            //Security.cls_Security Security = new Security.cls_Security();
            //string claveEncriptada = Security.Encripta("User=userBTS;Password=userBTS_01");            
            return View();
        }

        [HttpGet]        
        public JsonResult Views()
        {
            Resultado = Operacion.get_TrailerInformation();
            var respuesta = Json(Resultado, JsonRequestBehavior.AllowGet);
            return (respuesta);
        }

                
        public ActionResult History()
        {           
            return View();
        }

        [HttpGet]
        public JsonResult HistoryViews() 
        {       
            ResultadoHistorico = Historico.get_HistoryTrailerInformation();
            var respuesta = Json(ResultadoHistorico, JsonRequestBehavior.AllowGet);
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