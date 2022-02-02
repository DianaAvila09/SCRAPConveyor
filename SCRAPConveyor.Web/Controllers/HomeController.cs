using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Operacion;
using Historico;
using SCRAPConveyor.DB.Model;
using Microsoft.Reporting.WebForms;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Printing;
using System.Configuration;

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

        public ActionResult Pendientes(Int32 boleto = 0, Int32 idLinea = 0, DateTime? inicio = null, DateTime? fin = null)
        {
            SCRAPConveyorEntities db = new SCRAPConveyorEntities();
            inicio = inicio.HasValue ? inicio.Value : DateTime.Today.AddDays(-7);
            fin = fin.HasValue ? fin.Value : DateTime.Today;
            ViewBag.inicio = inicio.Value.ToString("MM/dd/yyyy");
            ViewBag.fin = fin.Value.ToString("MM/dd/yyyy");
            ViewBag.boleto = boleto;
            ViewBag.idLinea = idLinea;
            List<sp_GetList_BasculasFactura_Result> result = db.sp_GetList_BasculasFactura(boleto, idLinea, inicio, fin).ToList();
            return View(result);
        }

        public ActionResult ImprimirRemision(int boleto, int idLinea, DateTime? inicio = null, DateTime? fin = null)
        {
            using (SCRAPConveyorEntities db = new SCRAPConveyorEntities())
            {
                try
                {
                    String Impresora = ConfigurationManager.AppSettings["Impresora"].ToString();
                    var grupo = db.sp_GetList_BasculasFactura(boleto, idLinea, inicio, fin);
                    if (Impresora != "" && grupo != null)
                    {
                        string directorio = Server.MapPath("~/Temp/");
                        string archivo = Server.MapPath("~/Temp/remision.PDF");
                        LocalReport localReport = new LocalReport() { ReportPath = Server.MapPath("~/Formatos/remision.rdlc") };
                        ReportDataSource reportDataSource = new ReportDataSource("BasculaFactura", grupo);
                        localReport.DataSources.Add(reportDataSource);
                        string reportType = "PDF", mimeType, encoding, fileNameExtension;
                        Warning[] warnings;
                        string[] streams;
                        byte[] renderedBytes;
                        renderedBytes = localReport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                        try
                        {
                            if (!Directory.Exists(directorio))
                            {
                                Directory.CreateDirectory(directorio);
                            }
                            if (System.IO.File.Exists(archivo))
                            {
                                System.IO.File.Delete(archivo);
                            }
                            foreach (string item in Directory.GetFiles(directorio))
                            {
                                System.IO.File.Delete(item);
                            }
                        }
                        catch (Exception)
                        {

                        }


                        using (FileStream fs = new FileStream(archivo, FileMode.Create))
                        {
                            fs.Write(renderedBytes, 0, renderedBytes.Length);
                            fs.Close();
                            fs.Dispose();
                        }
                        

                        PrintPDF(Impresora, "Carta", archivo, 1);
                        //System.Drawing.Printing.PageSettings stt = new System.Drawing.Printing.PageSettings();
                        //stt.PrinterSettings.PrinterName = "Samsung M2070 Series";
                        //localReport.Print(stt);
                        //return File(renderedBytes, mimeType);

                        return RedirectToAction("Pendientes");
                        /*---------------*/
                        //byte[] result;
                        //using (var renderer = new WebReportRenderer("~/Formatos/remision.rdlc", "Report.pdf"))
                        //{
                        //    renderer.ReportInstance.DataSources.Add(new ReportDataSource("BasculaFactura", grupo));
                        //    result = renderer.RenderToBytesPDF();
                        //}
                        //File(result, "application/pdf", "Report.pdf");
                        //IntPtr ptr = new IntPtr(0);
                        //ptr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(result.Length);
                        //System.Runtime.InteropServices.Marshal.Copy(result, 0, ptr, result.Length);
                        ////RawPrinterHelper.SendBytesToPrinter("Samsung M2070 Series", ptr, renderedBytes.Length);
                        //RawPrinterHelper.SendBytesToPrinter("Microsoft Print to PDF", ptr, renderedBytes.Length);
                    }
                    else
                        return View("Error");
                }
                catch (Exception ex)
                {
                    
                    return View("Error");
                    
                }
                finally
                {
                    GC.Collect();
                }
            }
        }

        public bool PrintPDF( string printer, string paperName, string filename, int copies)
        {
            try
            {
                // Create the printer settings for our printer
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printer,
                    Copies = (short)copies,
                };

                // Create our page settings for the paper size selected
                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                };
                foreach (PaperSize paperSize in printerSettings.PaperSizes)
                {
                    if (paperSize.PaperName == paperName)
                    {
                        pageSettings.PaperSize = paperSize;
                        break;
                    }
                }
                // Now print the PDF document

                using (var document = PdfiumViewer.PdfDocument.Load(filename))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                        printDocument.Dispose();
                    }
                    document.Dispose();
                    
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}