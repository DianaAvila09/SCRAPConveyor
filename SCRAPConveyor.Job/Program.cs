using HtmlAgilityPack;
using SCRAPConveyor.Job.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SCRAPConveyor.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            { 
                string format = "dd/MMM/yyyy hh:mm tt";
                CultureInfo culture = CultureInfo.CreateSpecificCulture("es-MX"); 
                DateTimeStyles styles = DateTimeStyles.None;
                Console.WriteLine("SCRAP Conveyor Job - Obteniendo información");
                //string sUrl = "http://localhost:54828/test.html";
                string sUrl = "http://estdtcaseta3:8060/Revuelta/InformacionBascula";
                Encoding objEncoding = Encoding.GetEncoding("ISO-8859-1");
                CookieCollection objCookies = new CookieCollection();
                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(sUrl);
                getRequest.Method = "GET";
                getRequest.CookieContainer = new CookieContainer();
                getRequest.CookieContainer.Add(objCookies);
                string sGetResponse = string.Empty;
                using (HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse())
                {
                    objCookies = getResponse.Cookies;
                    using (System.IO.StreamReader srGetResponse = new System.IO.StreamReader(getResponse.GetResponseStream(), objEncoding))
                        sGetResponse = srGetResponse.ReadToEnd();
                }
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(sGetResponse);
                HtmlNodeCollection nodosTabla = document.DocumentNode.SelectNodes("//table");
                if (nodosTabla.Any())
                {
                    List<BasculaRevuelta> lista = new List<BasculaRevuelta>();
                    DataTable dt = HTMLTable_to_DataTable(nodosTabla.FirstOrDefault().OuterHtml);
                    Console.WriteLine(dt.Rows.Count.ToString() + " registros encontrados");
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            Int32.TryParse(row["Bascula PP"].ToString(), out Int32 _bascula);
                            Int32.TryParse(row["Boleto SP"].ToString(), out Int32 _boleto);
                            
                            //if (!DateTime.TryParse(row["FecHora PP"].ToString(), out DateTime _fechaCreacion))
                            if (!DateTime.TryParse(row["FecHora PP"].ToString(), culture, styles, out DateTime _fechaCreacion))
                            {
                                if (row["FecHora PP"].ToString() != "")
                                {
                                    Console.WriteLine("No pudo convertir FecHora PP: " + row["FecHora PP"].ToString());
                                    System.Threading.Thread.Sleep(2000);
                                }
                            };
                            DateTime? _fechaHoraSalida = null;
                            //if (DateTime.TryParse(row["FecHora SP"].ToString(), out DateTime _auxFechaHoraSalida)) 
                            if (DateTime.TryParse(row["FecHora SP"].ToString(), culture, styles, out DateTime _auxFechaHoraSalida))
                            {
                                _fechaHoraSalida = _auxFechaHoraSalida;
                            } else
                            {
                                if (row["FecHora SP"].ToString() != "")
                                {
                                    Console.WriteLine("No pudo convertir FecHora SP: " + row["FecHora SP"].ToString());
                                    System.Threading.Thread.Sleep(2000);
                                }
                            }
                            Decimal.TryParse(row["Peso 1a Pesada"].ToString(), out Decimal _Peso_1a_Pesada);
                            Decimal? _Peso_2a_Pesada = null;
                            if (Decimal.TryParse(row["Peso 2a Pesada"].ToString(), out Decimal _auxPeso_2a_Pesada))
                            {
                                _Peso_2a_Pesada = _auxPeso_2a_Pesada;
                            }
                            Decimal? _pesoTara = null;
                            if (Decimal.TryParse(row["Tara"].ToString(), out Decimal _auxPesoTara))
                            {
                                _pesoTara = _auxPesoTara;
                            }
                            lista.Add(new BasculaRevuelta() { 
                                bascula = _bascula,
                                boleto = _boleto,
                                chofer = row["Chofer"].ToString(),
                                empresa = row["Empresa"].ToString(),
                                //factura = _factura,
                                fechaCreacion = _fechaCreacion,
                                fechaHoraEntrada = _fechaCreacion,
                                fechaHoraSalida = _fechaHoraSalida,
                                //numFactura = "",
                                pesoEntrada = _Peso_1a_Pesada,
                                pesoSalida = _Peso_2a_Pesada,
                                pesoTara = _pesoTara,
                                placas = row["Placas"].ToString(),
                                tagCamion = row["Producto"].ToString(),
                                transportista = row["Transportista"].ToString(),
                                usuario = row["Usuario"].ToString(),
                            });
                        }
                        catch (Exception ex)
                        {
                            Log.LogException("foreach row", ex.Message);
                            Console.WriteLine(ex.Message);
                            if (ex.InnerException != null)
                            {
                                Log.LogException("Main inner exception", ex.InnerException.Message);
                            }
                            System.Threading.Thread.Sleep(2000);
                        }
                    }

                    using (SCRAPConveyorEntities db = new SCRAPConveyorEntities())
                    {
                        Int32 cont = 0;
                        foreach (BasculaRevuelta i in lista)
                        {
                            try
                            {
                                BasculaRevuelta registro = db.BasculaRevuelta.Where(x => x.boleto == i.boleto).FirstOrDefault();
                                registro = registro != null ? registro : new BasculaRevuelta(){ boleto = i.boleto };
                                registro.bascula = i.bascula;
                                registro.chofer = i.chofer;
                                registro.empresa = i.empresa;
                                registro.factura = i.factura;
                                registro.fechaCreacion = i.fechaCreacion;
                                registro.fechaHoraEntrada = i.fechaHoraEntrada;
                                registro.fechaHoraSalida = i.fechaHoraSalida;
                                registro.numFactura = i.numFactura;
                                registro.pesoEntrada = i.pesoEntrada;
                                registro.pesoSalida = i.pesoSalida;
                                registro.pesoTara = i.pesoTara;
                                registro.placas = i.placas;
                                registro.tagCamion = i.tagCamion;
                                registro.transportista = i.transportista;
                                registro.usuario = i.usuario;
                                db.Entry(registro).State = i.id > 0 ? System.Data.Entity.EntityState.Modified : System.Data.Entity.EntityState.Added;
                                cont++;
                            }
                            catch (Exception ex)
                            {
                                Log.LogException("foreach row", ex.Message);
                                Console.WriteLine(ex.Message);
                                if (ex.InnerException != null)
                                {
                                    Log.LogException("Main inner exception", ex.InnerException.Message);
                                }
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                        db.SaveChanges();
                        Console.WriteLine(cont.ToString() + " registros almacenados");
                        System.Threading.Thread.Sleep(2000);
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

        public static DataTable HTMLTable_to_DataTable(string HTML)
        {
            DataTable dt = null;
            DataRow dr = null;
            DataColumn dc = null;
            string TableExpression = "<table[^>]*>(.*?)</table>";
            string HeaderExpression = "<th[^>]*>(.*?)</th>";
            string RowExpression = "<tr[^>]*>(.*?)</tr>";
            string ColumnExpression = "<td[^>]*>(.*?)</td>";
            bool HeadersExist = false;
            int iCurrentColumn = 0;
            int iCurrentRow = 0;
            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match Table in Tables)
            {
                iCurrentRow = 0;
                HeadersExist = false;
                dt = new DataTable();
                if (Table.Value.Contains("<th"))
                {
                    HeadersExist = true;
                    MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    foreach (Match Header in Headers)
                        dt.Columns.Add(Header.Groups[1].ToString());
                }
                else
                {
                    for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                    {
                        dt.Columns.Add("Column " + iColumns);
                    }
                }
                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                foreach (Match Row in Rows)
                {
                    if (!(iCurrentRow == 0 & HeadersExist == true))
                    {
                        dr = dt.NewRow();
                        iCurrentColumn = 0;
                        MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        foreach (Match Column in Columns)
                        {
                            DataColumnCollection columns = dt.Columns;
                            if (!columns.Contains("Column " + iCurrentColumn))
                            {
                                dt.Columns.Add("Column " + iCurrentColumn);
                            }
                            dr[iCurrentColumn] = Column.Groups[1].ToString().Replace("/Ene/","/01/").Replace("/Feb/", "/02/").Replace("/Mar/", "/03/").Replace("/Abr/", "/04/").Replace("/May/", "/05/").Replace("/Jun/", "/06/").Replace("/Jul/", "/07/").Replace("/Ago/", "/08/").Replace("/Sep/", "/09/").Replace("/Oct/", "/10/").Replace("/Nov/", "/11/").Replace("/Dic/", "/12/").Replace("/Ene/", "/01/").Replace("/Feb/", "/02/").Replace("/Mar/", "/03/").Replace("/Abr/", "/04/").Replace("/May/", "/05/").Replace("/Jun/", "/06/").Replace("/Jul/", "/07/").Replace("/Ago/", "/08/").Replace("/Sep/", "/09/").Replace("/Oct/", "/10/").Replace("/Nov/", "/11/").Replace("/Dic/", "/12/").Replace("/Ene/", "/01/").Replace("/Feb/", "/02/").Replace("/Mar/", "/03/").Replace("/Abr/", "/04/").Replace("/May/", "/05/").Replace("/Jun/", "/06/").Replace("/Jul/", "/07/").Replace("/Ago/", "/08/").Replace("/Sep/", "/09/").Replace("/Oct/", "/10/").Replace("/Nov/", "/11/").Replace("/Dic/", "/12/");
                            iCurrentColumn += 1;
                        }
                        dt.Rows.Add(dr);
                    }
                    iCurrentRow += 1;
                }
            }
            return (dt);
        }
    }
}

