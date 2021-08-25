using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;


namespace Security
{
    /// <summary>
    /// Biblioteca Criptografica
    /// Salvador Flores Michel
    /// </summary>
    public class cls_Security
    {
        //Llaves!  // CAMBIARLAS POR OTRAS
        private byte[] tdesKey = { 88, 19, 12, 85, 83, 53, 90, 11, 32, 78, 74, 37, 55, 29, 35, 22, 99, 75, 7, 63, 18, 14, 81, 92 };
        private byte[] idesIV = { 78, 90, 48, 12, 34, 55, 03, 47 };

        //Constructor
        public cls_Security()
        { }

        /// <summary>
        /// Cadena de Conexión
        /// </summary>
        /// <returns></returns>
        public string CadenaDeConexion()
        {

            string strConnection = ConfigurationManager.AppSettings["connectionString"];
            string strSeguridad = ConfigurationManager.AppSettings["SQLKey"];
            if (!strConnection.EndsWith(";"))
            { strConnection += ";"; }
            if (strSeguridad.Length > 0)
            {
                strSeguridad = Desencripta(strSeguridad);
            }
            strConnection += strSeguridad;
            return strConnection;

        }


        /// <summary>
        /// Cadena de Conexión
        /// </summary>
        /// <returns></returns>
        public string CadenaEncrypt()
        {

            string strConnection = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
            string strSeguridad = System.Configuration.ConfigurationManager.AppSettings["SQLKey"];
            if (!strConnection.EndsWith(";"))
            { strConnection += ";"; }
            if (strSeguridad.Length > 0)
            {
                strSeguridad = Encripta("User=usiscg;Password=MOcg10ti");   //<- USAR PARA ENCRIPTAR LA LLAVE LA PRIMERA VEZ
            }

            return strSeguridad;

        }

        /// <summary>
        /// Encripta texto usando TripleDES
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public string Encripta(string strText)
        {
            //Crea el proveedor de encriptacion
            TripleDESCryptoServiceProvider EncodeMethod = new TripleDESCryptoServiceProvider();
            //Crea los buffers y streams 
            long lLen;
            int nRead, nReadTotal;
            byte[] buf = new byte[3];
            byte[] srcData;
            byte[] encData;
            System.IO.MemoryStream sin;
            System.IO.MemoryStream sout;
            CryptoStream encStream;
            //Llena buffer de entrada
            srcData = System.Text.ASCIIEncoding.ASCII.GetBytes(strText);
            sin = new MemoryStream();
            sin.Write(srcData, 0, srcData.Length);
            sin.Position = 0;
            sout = new MemoryStream();
            //Inicializa la llave y el vector 
            EncodeMethod.Key = tdesKey;
            EncodeMethod.IV = idesIV;
            //Llena el stream de entrada
            encStream = new CryptoStream(sout,
                EncodeMethod.CreateEncryptor(),
                CryptoStreamMode.Write);
            lLen = sin.Length;
            nReadTotal = 0;
            while (nReadTotal < lLen)
            {
                nRead = sin.Read(buf, 0, buf.Length);
                encStream.Write(buf, 0, nRead);
                nReadTotal += nRead;
            }
            encStream.Close();
            //Llena el stream de salida
            encData = sout.ToArray();
            return System.Convert.ToBase64String(encData);
        }

        /// <summary>
        /// Desencripta texto encriptado con TripleDES
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public string Desencripta(string strText)
        {
            TripleDESCryptoServiceProvider EncodeMethod = new TripleDESCryptoServiceProvider();
            long lLen;
            int nRead, nReadTotal;
            byte[] buf = new byte[3];
            byte[] decData;
            byte[] encData;
            System.IO.MemoryStream sin;
            System.IO.MemoryStream sout;
            CryptoStream decStream;
            encData = System.Convert.FromBase64String(strText);
            sin = new MemoryStream(encData);
            sout = new MemoryStream();
            EncodeMethod.Key = tdesKey;
            EncodeMethod.IV = idesIV;
            decStream = new CryptoStream(sin,
                EncodeMethod.CreateDecryptor(),
                CryptoStreamMode.Read);
            lLen = sin.Length;
            nReadTotal = 0;
            while (nReadTotal < lLen)
            {
                nRead = decStream.Read(buf, 0, buf.Length);
                if (0 == nRead) break;

                sout.Write(buf, 0, nRead);
                nReadTotal += nRead;
            }
            decStream.Close();
            decData = sout.ToArray();
            ASCIIEncoding ascEnc = new ASCIIEncoding();
            return ascEnc.GetString(decData);
        }
    }

}