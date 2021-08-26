using System.Data.SqlClient;
using Errores;
using System.Web.UI.WebControls;


namespace Mensajes
{
    /// <summary>
    /// Mensajes del Sistema
    /// Salvador Flores Michel
    /// </summary>
    public class cls_Mensajes
    {

        public cls_Mensajes()
        { }

        /// <summary>
        /// Trae mensaje directo
        /// </summary>
        /// <param name="ClaveMensaje"></param>
        /// <returns></returns>
        public string TraeMensaje(int ClaveMensaje)
        {
            string Mensaje = TraeMensaje(ClaveMensaje, "");
            return Mensaje;
        }

        /// <summary>
        /// Trae mensaje compuesto
        /// </summary>
        /// <param name="ClaveMensaje"></param>
        /// <param name="SubTexto"></param>
        /// <returns></returns>
        public string TraeMensaje(int ClaveMensaje, string SubTexto)
        {
            string MensajeError = "";

            switch (ClaveMensaje)
            {
                case 1:
                    MensajeError = "El Servidor no existe o acceso negado";
                    break;

                case 2:
                    MensajeError = "El Usuario no existe o contraseña incorrecta";
                    break;

                case 3:
                    MensajeError = "No se ha iniciado sesión";
                    break;

                case 4:
                    MensajeError = "No tiene permisos suficientes para ejecutar esta acción";
                    break;

                case 10:
                    MensajeError = "Hay registros relacionados";
                    break;

                case 11:
                    MensajeError = "Registro repetido";
                    break;

                case 12:
                    MensajeError = "La contraseña y su confirmación no coinciden";
                    break;

                case 13:
                    MensajeError = "La contraseña actual no coincide";
                    break;

                case 14:
                    MensajeError = "Se requiere de una contraseña";
                    break;

                case 15:
                    MensajeError = "Se requiere de una identificación para el usuario";
                    break;

                case 16:
                    MensajeError = "El servidor está ocupado o no disponible";
                    break;

                case 17:
                    MensajeError = "Se ha interrumpido la comunicación";
                    break;

                case 18:
                    MensajeError = "El archivo no existe o se encuentra bloqueado";
                    break;

                case 19:
                    MensajeError = "Espacio insuficiente en disco";
                    break;

                case 20:
                    MensajeError = "El nombre del archivo no está permitido";
                    break;

                case 100:
                    MensajeError = "Campo requerido";
                    break;

                case 101:
                    MensajeError = "Datos actualizados con éxito";
                    break;

                case 102:
                    MensajeError = "Espere mientras se procesa su petición...";
                    break;

                case 103:
                    MensajeError = "No hay elementos en la lista";
                    break;

                case 104:
                    MensajeError = "Se requiere(n) valor(es) numérico(s) en: ";
                    break;

                case 105:
                    MensajeError = "La información no coincide con el tipo de valor esperado";
                    break;

                case 106:
                    MensajeError = "La información está incompleta";
                    break;
            }
            return MensajeError;
        }

        /// <summary>
        /// Regresa error de SQL
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        public string MuestraErrorSQL(SqlException err)
        {
            cls_Errores errSQL = new cls_Errores();
            return errSQL.RegresaSQLError(err);
        }

        #region Manejo de Mensajes en Pagina
        /// <summary>
        /// Muestra mensaje de error de la Base de Datos
        /// </summary>
        /// <param name="err">Error de SQL</param>
        public void MuestraError(ref Label labelError, SqlException err)
        {
            cls_Errores errSQL = new cls_Errores();
            labelError.ForeColor = System.Drawing.Color.Red;
            labelError.Text = errSQL.RegresaSQLError(err);
        }

        /// <summary>
        /// Muestra mensaje de error de Usuario
        /// </summary>
        /// <param name="strError"></param>
        public void MuestraError(ref Label labelError, string strError)
        {
            labelError.ForeColor = System.Drawing.Color.Red;
            labelError.Text = strError;
        }

        /// <summary>
        /// Muestra error del sistema
        /// </summary>
        /// <param name="MensajeID">ID del error</param>
        public void MuestraError(ref Label labelError, int MensajeID)
        {
            labelError.ForeColor = System.Drawing.Color.Red;
            labelError.Text = TraeMensaje(MensajeID);
        }

        /// <summary>
        /// Muestra avisos de texto
        /// </summary>
        /// <param name="strMensaje">Texto del Mensaje</param>
        public void MuestraMensaje(ref Label labelError, string strMensaje)
        {
            labelError.ForeColor = System.Drawing.Color.Green;
            labelError.Text = strMensaje;
        }

        /// <summary>
        /// Muestra mensajes del sistema
        /// </summary>
        /// <param name="MensajeID">ID del mensaje</param>
        public void MuestraMensaje(ref Label labelError, int MensajeID)
        {
            labelError.ForeColor = System.Drawing.Color.Green;
            labelError.Text = TraeMensaje(MensajeID);
        }
        #endregion
    }
}
