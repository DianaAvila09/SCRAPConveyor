using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Operacion;
using Historico;
using SCRAPConveyor.DB.Model;

namespace SCRAPConveyor.Web.Controllers
{
    public class HomeController : Controller
    {
        cls_Operacion Operacion = new cls_Operacion();
        cls_Historico Historico = new cls_Historico();

        List<cls_Operacion.TrailerInformation> Resultado = new List<cls_Operacion.TrailerInformation>();
        List<sp_GetList_HistoryReport_Result> ResultadoHistorico = new List<sp_GetList_HistoryReport_Result>();


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
            SCRAPConveyorEntities db = new SCRAPConveyorEntities();
            var today = DateTime.Today;
            DateTime inicio = today.AddDays(-7);
            DateTime fin = today.AddDays(0);
            ViewBag.inicio = inicio.ToString("MM/dd/yyyy");
            ViewBag.fin = fin.ToString("MM/dd/yyyy"); 
             
            //ResultadoHistorico = Historico.get_HistoryTrailerInformation();
            List<sp_GetList_HistoryReport_Result> result = db.sp_GetList_HistoryReport(inicio, fin).ToList();
            return View(result);
        }

        [HttpGet]
        public JsonResult HistoryViews() 
        {
            var today = DateTime.Today;
            DateTime inicio = today.AddDays(0);
            DateTime fin = today.AddDays(1);

            //ResultadoHistorico = Historico.get_HistoryTrailerInformation();
            var respuesta = Json(ResultadoHistorico, JsonRequestBehavior.AllowGet);
            return (respuesta);
        }

        public ActionResult Datefilter(DateTime inicio, DateTime fin)
        {
            SCRAPConveyorEntities db = new SCRAPConveyorEntities();
            ViewBag.inicio = inicio.ToString("MM/dd/yyyy");
            ViewBag.fin = fin.ToString("MM/dd/yyyy");
            fin = fin.AddHours(23);
            fin = fin.AddMinutes(59);

            //ResultadoHistorico = Historico.get_HistoryTrailerInformation();
            List<sp_GetList_HistoryReport_Result> result = db.sp_GetList_HistoryReport(inicio, fin).ToList();

            //var result = ResultadoHistorico.Where(a => a.creationDate >= inicio && a.creationDate <= fin).OrderBy(b => b.creationDate).ToList();

            return View(result);
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