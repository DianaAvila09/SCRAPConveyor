using Base_de_Datos;
using System.Data;

namespace RepositorioSQL
{
    public class cls_RepositorioSQL
    {
        public cls_Coneccion BD = new cls_Coneccion();
        DataSet oDs = new DataSet();

        public cls_RepositorioSQL()
        {

        }

        /// <summary>
        /// Selecciona de cualquier catalogo todos sus registros o uno en particular
        /// </summary>
        /// <param name="pCode">Code a Utilizar</param>
        /// <param name="pTrailerNumber">Numero de Trailer</param>
        /// <returns>Registro(s)</returns>
        public DataSet Sel_TrailerInformation(string pCode = "", int pTrailerNumber = 0)
        {
            BD.SetCommand("SP_SelTrailerInformation");
            if (pCode != "") BD.CreateParameter("@Code", pCode, 10);
            if (pTrailerNumber != 0) BD.CreateParameter("@TrailerNumber", pTrailerNumber);
            return BD.getDataSet();
        }

    }
}
