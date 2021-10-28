using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SCRAPConveyor.DB.Model;
using SCRAPConveyor.Facturacion.Model;

namespace SCRAPConveyor.Facturacion
{
    class Program
    {
        static void Main(string[] args)
        {
            documentos();
            facturas();
        }

        protected static void documentos()
        {
            try
            {
                using (SCRAPConveyorEntities db = new SCRAPConveyorEntities())
                {
                    var registros = (from b in db.BasculaRevuelta.Where(x => x.documento != true && x.fechaHoraSalida != null)
                                     join p in db.PrecioSCRAP on b.producto equals p.tipo
                                     join f in db.Factura on b.boleto equals f.boleto
                                     select new { b.boleto, b.producto, cantidad = b.pesoSalida - b.pesoTara, p.precio, p.moneda, f.tipoMaterial, f.descSAP }).ToList();
                    int cont = 0;
                    foreach (var registro in registros) 
                    {
                        cont++;
                        Console.WriteLine("Generando documento para boleto " + registro.boleto + ", registro " + cont.ToString() + " de " + registros.Count.ToString());
                        try
                        { 
                            using (SCRAPConveyorEntities ctx = new SCRAPConveyorEntities())
                            {
                                bool errores = false;
                                SAP sap = new SAP();
                                List<IT_MATERIALES> materiales = new List<IT_MATERIALES>();
                                switch (ConfigurationManager.AppSettings["Entorno"].ToUpper())
                                {
                                    case "QAS":
                                        materiales.Add(new IT_MATERIALES() { MATERIAL = registro.descSAP, CANTIDAD = registro.cantidad ?? 0, DESCRIPCION = registro.tipoMaterial, MONEDA = registro.moneda, PRECIO = Math.Round(registro.precio ?? 0, 2), UNIDAD_PRECIO = 1000 });
                                        break;
                                    case "PRD":
                                        materiales.Add(new IT_MATERIALES() { MATERIAL = registro.descSAP, CANTIDAD = registro.cantidad ?? 0, DESCRIPCION = registro.tipoMaterial, MONEDA = registro.moneda, PRECIO = Math.Round(registro.precio ?? 0, 2), UNIDAD_PRECIO = 1000 });
                                        break;
                                    default:
                                        break;
                                }
                                Tuple<List<ET_MENSAJES>, string> documento = sap.ZFIFM_CREAR_PED_SCRAP("1841", "20", "20", "10007", DateTime.Today.ToString("MMM-yy"), materiales);
                                if (documento != null && documento.Item1.Any())
                                {
                                    List<BasculaRevuelta_Log> logs = new List<BasculaRevuelta_Log>();
                                    foreach (var item in documento.Item1)
                                    {
                                        logs.Add(new BasculaRevuelta_Log() { bapi = "ZFIFM_CREAR_PED_SCRAP", boleto = registro.boleto ?? 0, fecha_ejecucion = DateTime.Now, message = item.MESSAGE, type = item.TYPE });
                                        if (item.TYPE.ToUpper() == "E")
                                            errores = true;
                                    }
                                    ctx.BasculaRevuelta_Log.AddRange(logs);
                                    ctx.SaveChanges();
                                }
                                if (!errores)
                                {
                                    BasculaRevuelta bascula_revuelta = ctx.BasculaRevuelta.Where(x => x.boleto == registro.boleto).FirstOrDefault();
                                    bascula_revuelta.documento = true;
                                    bascula_revuelta.numDocumento = documento.Item2;
                                    bascula_revuelta.fechaDocumento = DateTime.Now;
                                    ctx.Entry(bascula_revuelta).State = System.Data.Entity.EntityState.Modified;
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.LogException("Main", ex.Message);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Log.LogException("Main", ex.Message);
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Log.LogException("Main inner exception", ex.InnerException.Message);
                    Log.LogException("Main inner exception", ex.InnerException.Source);
                    Log.LogException("Main inner exception", ex.InnerException.StackTrace);
                    Log.LogException("Main inner exception", ex.InnerException.Data.Values.ToString());
                    if (ex.InnerException.InnerException != null)
                    {
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.Message);
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.Source);
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.StackTrace);
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.Data.Values.ToString());
                    }
                }
                System.Threading.Thread.Sleep(2000);
            }
        }

        protected static void facturas()
        {
            try
            {
                using (SCRAPConveyorEntities db = new SCRAPConveyorEntities())
                {
                    var registros = (from b in db.BasculaRevuelta.Where(x => x.documento == true && x.factura != true && x.fechaHoraSalida != null && x.numDocumento != null) select b).ToList();
                    int cont = 0;
                    foreach (var registro in registros)
                    {
                        Console.WriteLine("Generando documento para boleto " + registro.boleto + ", registro " + cont.ToString() + " de " + registros.Count.ToString());
                        try
                        {
                            using (SCRAPConveyorEntities ctx = new SCRAPConveyorEntities())
                            {
                                bool errores = false;
                                SAP sap = new SAP();
                                Tuple<List<ET_MENSAJES>, List<ET_DOCUMENTOS>> factura = sap.ZFIFM_CREAR_FRA_SCRAP("1841", new List<String>() { registro.numDocumento });
                                if (factura != null && factura.Item1.Any())
                                {
                                    List<BasculaRevuelta_Log> logs = new List<BasculaRevuelta_Log>();
                                    foreach (var item in factura.Item1)
                                    {
                                        logs.Add(new BasculaRevuelta_Log() { bapi = "ZFIFM_CREAR_PED_SCRAP", boleto = registro.boleto ?? 0, fecha_ejecucion = DateTime.Now, message = item.MESSAGE, type = item.TYPE });
                                        if (item.TYPE.ToUpper() == "E")
                                            errores = true;
                                    }
                                    ctx.BasculaRevuelta_Log.AddRange(logs);
                                    ctx.SaveChanges();
                                }
                                if (!errores)
                                {
                                    List<ET_DOCUMENTOS> documentos = new List<ET_DOCUMENTOS>();
                                    foreach (var doc in factura.Item2)
                                    {
                                        BasculaRevuelta bascula_revuelta = ctx.BasculaRevuelta.Where(x => x.boleto == registro.boleto).FirstOrDefault();
                                        bascula_revuelta.factura = true;
                                        bascula_revuelta.numFactura = doc.FACTURA;
                                        bascula_revuelta.fechaFactura = DateTime.Now;
                                        ctx.Entry(bascula_revuelta).State = System.Data.Entity.EntityState.Modified;
                                        ctx.SaveChanges();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.LogException("Main", ex.Message);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Log.LogException("Main", ex.Message);
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Log.LogException("Main inner exception", ex.InnerException.Message);
                    Log.LogException("Main inner exception", ex.InnerException.Source);
                    Log.LogException("Main inner exception", ex.InnerException.StackTrace);
                    Log.LogException("Main inner exception", ex.InnerException.Data.Values.ToString());
                    if (ex.InnerException.InnerException != null)
                    {
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.Message);
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.Source);
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.StackTrace);
                        Log.LogException("Main inner exception", ex.InnerException.InnerException.Data.Values.ToString());
                    }
                }
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
