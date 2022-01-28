using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Operacion;
using Historico;
using SCRAPConveyor.DB.Model;
using Microsoft.Reporting.WebForms;

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

        public ActionResult Pendientes(Int32 boleto = 0, Int32 idLinea = 0)
        {
            SCRAPConveyorEntities db = new SCRAPConveyorEntities();
            ViewBag.boleto = boleto;
            ViewBag.idLinea = idLinea;
            List<sp_GetList_BasculasFactura_Result> result = db.sp_GetList_BasculasFactura(boleto, idLinea).ToList();
            return View(result);
        }

        public ActionResult ImprimirRemision(int boleto, int idLinea)
        {
            using (SCRAPConveyorEntities db = new SCRAPConveyorEntities())
            {
                var grupo = db.sp_GetList_BasculasFactura(boleto, idLinea);
                if (grupo != null)
                {
                    //declarar el objeto de la clase LocalReport
                    LocalReport localReport = new LocalReport();
                    localReport.ReportPath = Server.MapPath("~/Formatos/remision.rdlc");
                    //Llenar los 4 parámetros que creamos en el reporte
                    //ReportParameter[] parametros = new ReportParameter[4];
                    //parametros[0] = new ReportParameter("ProgramaEducativo", grupo.Cuatrimestre.ProgramaEducativo.NombreCorto.Trim());
                    //parametros[1] = new ReportParameter("Cuatrimestre", grupo.Cuatrimestre.PeriodoInicio.Trim() + " - " +
                    //grupo.Cuatrimestre.PeriodoFin.Trim() + " " + grupo.Cuatrimestre.Anio);
                    //parametros[2] = new ReportParameter("Grupo", grupo.Nombre.Trim());
                    //parametros[3] = new ReportParameter("Tutor", grupo.Tutor.Nombre.Trim() + " " + grupo.Tutor.Apellidos.Trim());
                    ////agregar los parámetros al reporte
                    //localReport.SetParameters(parametros);
                    //Preparar el DataSource del reporte
                    //aquí invocamos al método ListaAlumnos que agregamos a la clase Alumno
                    //pasandole como valor de parámetro el id del grupo que se quiere imprimir
                    //No hemos hablado de vistas o procedimientos almacenados, sino, aquí se invocarían directamente
                    ReportDataSource reportDataSource = new ReportDataSource("BasculaFactura", grupo);
                    //Ahora pasamos los datos al reporte
                    localReport.DataSources.Add(reportDataSource);
                    //declaramos las variables de configuración para el reporte
                    string reportType = "PDF";
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    //En la configuración del reporte debe especificarse para el tipo de reporte
                    //consulte el sitio para más información
                    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                    Warning[] warnings;
                    string[] streams;
                    byte[] renderedBytes;
                    //Transformar el reporte a bytes para transferirlo como flujo
                    renderedBytes = localReport.Render(reportType, null, out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                    //enviar el archivo al cliente (navegador)
                    return File(renderedBytes, mimeType);
                }
                else
                    return View("Error");
            }
        }

    }
}