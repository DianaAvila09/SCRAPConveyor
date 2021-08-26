using System.Text;
using System.Data.SqlClient;
using Mensajes;

namespace Errores
{
    public class cls_Errores
    {
        public cls_Errores()
        { }


        /// <summary>
        /// Resuelve mensajes de error de SQL
        /// </summary>
        /// <param name="NumeroError">Numero del Error</param>
        /// <param name="MensajeError">Descripcion del Error</param>
        /// <returns></returns>
        private string ResuelveSQLError(int NumeroError, string MensajeError)
        {
            cls_Mensajes mensajes = new cls_Mensajes();
            string strMensajeResuelto;
            string strMensajeAux;
            int nPosIni = 0;
            int nPosFin = 0;

            switch (NumeroError)
            {
                case 11:
                    //Error de red o el servidor no existe
                    strMensajeResuelto = mensajes.TraeMensaje(14);
                    break;

                case 17:
                    //Servidor no existe o aceso negado
                    strMensajeResuelto = mensajes.TraeMensaje(1);
                    break;

                case 53:
                    //No se pudo contactar al Servidor
                    strMensajeResuelto = mensajes.TraeMensaje(16);
                    break;

                case 515:
                    //Insert NULL
                    nPosIni = MensajeError.IndexOf("'");
                    strMensajeAux = mensajes.TraeMensaje(106);
                    if (nPosIni > 0)
                    {
                        nPosFin = MensajeError.IndexOf("'", nPosIni + 1);
                        if (nPosFin > 0)
                        { strMensajeAux += " en " + MensajeError.Substring(nPosIni + 1, nPosFin - nPosIni - 1); }
                    }
                    strMensajeResuelto = strMensajeAux;
                    break;

                case 547:
                    //Registros relacionados
                    strMensajeResuelto = mensajes.TraeMensaje(10);
                    break;

                case 2601:
                    //Registros repetidos					
                    strMensajeResuelto = mensajes.TraeMensaje(11);
                    break;

                case 2627:
                    //Constraint violation
                    strMensajeResuelto = mensajes.TraeMensaje(10);
                    break;

                case 15211:
                    //contraseña actual incorrecta
                    strMensajeResuelto = mensajes.TraeMensaje(2);
                    break;

                case 229:
                case 15247:
                    //No tiene permisos
                    strMensajeResuelto = mensajes.TraeMensaje(4);
                    break;

                case 18456:
                    //Usuario no existe o contraseña incorrecta
                    strMensajeResuelto = mensajes.TraeMensaje(2);
                    break;

                default:
                    strMensajeResuelto = MensajeError;
                    break;
            }
            return strMensajeResuelto;
        }

        /// <summary>
        /// Regresa un error de SQL resuelto
        /// </summary>
        /// <param name="exErr"></param>
        /// <returns></returns>
        public string RegresaSQLError(SqlException exErr)
        {
            StringBuilder strMsgError = new StringBuilder();
            //Por cada error
            foreach (SqlError error in exErr.Errors)
            {
                if (error.Class >= 19)
                {
                    strMsgError.Insert(0, "ERROR FATAL: " + ResuelveSQLError(error.Number, error.Message));
                }
                if (error.Class == 17 | error.Class == 18)
                {
                    strMsgError.Insert(0, "RECURSOS DEL SISTEMA: " + ResuelveSQLError(error.Number, error.Message));
                }
                if (error.Class >= 11 & error.Class <= 16)
                {
                    if (strMsgError.Length > 1)
                    { strMsgError.Insert(strMsgError.Length, ". "); }
                    strMsgError.Insert(strMsgError.Length, ResuelveSQLError(error.Number, error.Message));
                }
                if (error.Class <= 10)
                {
                    //Solo si es el unico
                    if (exErr.Errors.Count == 1)
                    {
                        strMsgError.Insert(strMsgError.Length, error.Message);
                    }
                }
            }
            return strMsgError.ToString();
        }
    }
}

