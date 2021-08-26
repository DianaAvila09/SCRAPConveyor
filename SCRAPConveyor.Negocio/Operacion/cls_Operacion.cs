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
            public double filingLevel { get; set; }
            public int trailerNumber { get; set; }
            public string code { get; set; }

        }

        /// <summary>
        /// Selecciona de cualquier catalogo todos sus registros o uno en particular
        /// </summary>
        /// <param name="pCode">Code a Utilizar</param>       
        /// <returns>Registro(s)</returns>
        public DataSet Sel_TrailerInformation(string pCode)
        {
            BD.SetCommand("SP_SelTrailerInformation");
            if (pCode != "") BD.CreateParameter("@Code", pCode, 10);
            return BD.getDataSet();
        }

        public List<TrailerInformation> get_TrailerInformation(string pCode)
        {
            List<TrailerInformation> lTrailerInformation = new List<TrailerInformation>();
            oDs = Sel_TrailerInformation(pCode);
            

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
                                    filingLevel = Convert.ToDouble(row["filingLevel"].ToString()),
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

            var js = new JavaScriptSerializer();
            //Context.Response.Write(js.Serialize(json));


            return lTrailerInformation;

        }

    }
}
