using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SCRAPConveyor.Job
{
    class Log
    {
        private static void WriteLog(string detail)
        {
            try
            {
                using (StreamWriter file =
                    new System.IO.StreamWriter(ConfigurationManager.AppSettings["LogPath"].ToString(), true))
                {
                    string line = "";
                    line += detail;
                    file.WriteLine(line);
                }
            }
            catch (Exception ex) { }
        }

        public static void LogExito(string metodo, string mensaje)
        {
            string title = "[" + DateTime.Now.ToString() + "] - " + metodo + ":";
            string body = "Se completó exitosamente: " + mensaje;

            WriteLog(title);
            WriteLog(body);
            WriteLog("----------------------------------------------------------------------------------------------------------------------");
        }

        public static void LogException(string metodo, string excepcion)
        {
            string title = "[" + DateTime.Now.ToString() + "] - " + metodo + ":";
            string body = excepcion;

            WriteLog(title);
            WriteLog(body);
            WriteLog("----------------------------------------------------------------------------------------------------------------------");
        }
    }
}
