using Base_de_Datos;
using System.Data;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;
using System.Linq;


namespace Historico
{
    public class cls_Historico
    {
        public cls_Coneccion BD = new cls_Coneccion();
        public RepositorioSQL.cls_RepositorioSQL cls_RepositorioSQL = new RepositorioSQL.cls_RepositorioSQL();
        DataSet oDs = new DataSet();

        public cls_Historico()
        {

        }

        public class HistoryTrailerInformation
        {
            public string material { get; set; }
            public string tolvaNumber { get; set; }
            public bool releaseMaterial { get; set; }
            public bool materialOnTheWay { get; set; }
            public bool mixedMaterial { get; set; }
            public bool reqTrailerExchange { get; set; }
            public double fillingLevel { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public DateTime startPlant { get; set; }
            public DateTime? endPlant { get; set; }
            public decimal startWeight { get; set; }
            public decimal? endWeight { get; set; }
            public DateTime creationDate { get; set; }
            public string code { get; set; }


        }

        //public List<HistoryTrailerInformation> get_HistoryTrailerInformation(string pCode = "", int pTrailerNumber = 0)
        //{
        //    List<HistoryTrailerInformation> lTrailerInformation = new List<HistoryTrailerInformation>();
        //    oDs = cls_RepositorioSQL.Sel_HistoryTrailerInformation();


        //    try
        //    {
        //        if (oDs.Tables[0].Rows.Count > 0)
        //        {
        //            DataTable dt = new DataTable();
        //            dt = oDs.Tables[0];

        //            lTrailerInformation = (from DataRow row in dt.Rows
        //                                   select new HistoryTrailerInformation
        //                                   {
        //                                       material = row["material"].ToString(),
        //                                       tolvaNumber = row["tolvaNumber"].ToString(),

        //                                       releaseMaterial = Convert.ToBoolean(row["releaseMaterial"]),
        //                                       materialOnTheWay = Convert.ToBoolean(row["materialOnTheWay"]),
        //                                       mixedMaterial = Convert.ToBoolean(row["mixedMaterial"]),
        //                                       reqTrailerExchange = Convert.ToBoolean(row["reqTrailerExchange"]),
        //                                       fillingLevel = Convert.ToDouble(row["fillingLevel"].ToString()),

        //                                       startDate = Convert.ToDateTime(row["startDate"]),
        //                                       endDate = Convert.ToDateTime(row["endDate"]),
        //                                       startPlant = Convert.ToDateTime(row["startPlant"]),                                               
        //                                       //endPlant = Convert.ToDateTime(row["endPlant"]),
        //                                       startWeight = Convert.ToDecimal(row["startWeight"].ToString()),
        //                                       //endWeight = Convert.ToDecimal(row["endWeight"].ToString()),
        //                                       creationDate = Convert.ToDateTime(row["creationDate"].ToString()),

        //                                       code = row["code"].ToString()
        //                                   }).ToList();
        //        }

        //        oDs.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return lTrailerInformation;

        //}

    }
}
