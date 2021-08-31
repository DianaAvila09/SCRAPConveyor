using Base_de_Datos;
using System.Data;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;
using System.Linq;

namespace Operacion
{
    public class cls_Operacion
    {
        public cls_Coneccion BD = new cls_Coneccion();
        public RepositorioSQL.cls_RepositorioSQL cls_RepositorioSQL = new RepositorioSQL.cls_RepositorioSQL();
        DataSet oDs = new DataSet();

        public cls_Operacion()
        {

        }

        public class TrailerInformation
        {
            public int id { get; set; }
            public bool releaseMaterial { get; set; }
            public bool materialOnTheWay { get; set; }
            public bool mixedMaterial { get; set; }
            public bool reqTrailerExchange { get; set; }
            public decimal fillingLevel { get; set; }
            public int trailerNumber { get; set; }
            public string code { get; set; }

        }

        

        public List<TrailerInformation> get_TrailerInformation(string pCode = "", int pTrailerNumber = 0)
        {
            List<TrailerInformation> lTrailerInformation = new List<TrailerInformation>();
            oDs = cls_RepositorioSQL.Sel_TrailerInformation(pCode, pTrailerNumber);
            

            try
            {
                if (oDs.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = oDs.Tables[0];

                    lTrailerInformation = (from DataRow row in dt.Rows
                                select new TrailerInformation
                                {
                                    id = Convert.ToInt32(row["Id"].ToString()),
                                    releaseMaterial = Convert.ToBoolean(row["releaseMaterial"]),
                                    materialOnTheWay = Convert.ToBoolean(row["materialOnTheWay"]),
                                    mixedMaterial = Convert.ToBoolean(row["mixedMaterial"]),
                                    reqTrailerExchange = Convert.ToBoolean(row["reqTrailerExchange"]),
                                    fillingLevel = Convert.ToDecimal(row["fillingLevel"].ToString()),
                                    trailerNumber = Convert.ToInt32(row["trailerNumber"].ToString()),
                                    code = row["code"].ToString()
                                }).ToList();
                }

                oDs.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lTrailerInformation;

        }

    }
}
