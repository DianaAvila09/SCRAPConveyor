using System;
using System.Data;
using System.Data.SqlClient;
using Security;

namespace Base_de_Datos
{
    /// <summary>
    /// ADO.NET data access using the SQL Server Managed Provider.
    /// </summary>
    public class cls_Coneccion : IDisposable
    {
        // connection to data source
        private SqlConnection con;
        private SqlCommand command;
        private cls_Security Security = new cls_Security();
        private String ConnectionString;
        /// <summary>
        /// Open the connection.
        /// </summary>
        public void Open()
        {
            try
            {
                // open connection
                if (con == null)
                {
                    ConnectionString = Security.CadenaDeConexion();
                    con = new SqlConnection(ConnectionString);
                    con.Open();

                    //con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
                    //con.Open();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al abrir Conexcion. Error = [" + ex.Message + "]");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        /// <summary>
        /// Close the connection.
        /// </summary>
        public void Close()
        {
            if (con != null)
                con.Close();
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Dispose()
        {
            // make sure connection is closed
            if (con != null)
            {
                con.Dispose();
                con = null;
            }
        }


        /// <summary>
        /// Asigna Comando a ejecutar
        /// </summary>
        /// <param name="Comando"></param>
        public void SetCommand(string Comando)
        {
            if (command == null)
            {
                this.Open();
                command = new SqlCommand(Comando, con);
                command.CommandTimeout = 36000;
            }
            else
            {
                try
                { command.Cancel(); }
                catch (Exception)
                { }
                try
                { command.Parameters.Clear(); }
                catch (Exception)
                { }
            }
            command.CommandText = Comando;
            command.CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// Asigna Comando a ejecutar con Timeout extendido
        /// </summary>
        /// <param name="Comando"></param>
        public void SetCommand(string Comando, int TimeOut)
        {
            if (command == null)
            {
                this.Open();
                command = new SqlCommand(Comando, con);
                command.CommandTimeout = TimeOut;
            }
            else
            {
                try
                { command.Cancel(); }
                catch (Exception)
                { }
                try
                { command.Parameters.Clear(); }
                catch (Exception)
                { }
            }
            command.CommandText = Comando;
            command.CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// Crea Parametro de Salida de tipo Caracter
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="strParameter"></param>
        /// <param name="nLenght"></param>
        /// <returns></returns>
        public SqlParameter CreateOutputParameter(string strParameterName, string strParameter, int nLenght)
        {
            SqlParameter parameter = new SqlParameter(strParameterName, System.Data.SqlDbType.NVarChar, nLenght);
            parameter.Value = strParameter;
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Crea Parametro de Salida de tipo Entero
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="nParameter"></param>
        /// <returns></returns>
        public SqlParameter CreateOutputParameter(string strParameterName, int nParameter)
        {
            SqlParameter parameter = new SqlParameter(strParameterName, System.Data.SqlDbType.Int, 4);
            parameter.Value = nParameter;
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Crea Parametro de Salida de tipo Entero Largo
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="nParameter"></param>
        /// <returns></returns>
        public SqlParameter CreateOutputParameter(string strParameterName, long nParameter)
        {
            SqlParameter parameter = new SqlParameter(strParameterName, System.Data.SqlDbType.BigInt, 8);
            parameter.Value = nParameter;
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Crea Parametro de Salida de tipo Lógico
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="bParameter"></param>
        /// <returns></returns>
        public SqlParameter CreateOutputParameter(string strParameterName, bool bParameter)
        {
            SqlParameter parameter = new SqlParameter(strParameterName, System.Data.SqlDbType.Bit, 1);
            parameter.Value = bParameter;
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Crea Valor de Retorno de tipo Entero
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <returns></returns>
        public SqlParameter CreateReturnValueParameter(string strParameterName)
        {
            SqlParameter parameter = new SqlParameter(strParameterName, System.Data.SqlDbType.Int);
            parameter.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Entero
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="nParameter"></param>
        public void CreateParameter(string strParameterName, int nParameter)
        {
            command.Parameters.Add(strParameterName, System.Data.SqlDbType.Int, 4).Value = nParameter;
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Entero largo
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="nParameter"></param>
        public void CreateParameter(string strParameterName, long nParameter)
        {
            command.Parameters.Add(strParameterName, System.Data.SqlDbType.Int, 8).Value = nParameter;
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Doble
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="nParameter"></param>
        public void CreateParameter(string strParameterName, double nParameter)
        {
            command.Parameters.Add(strParameterName, System.Data.SqlDbType.Money, 8).Value = nParameter;
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Caracter
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="strParameter"></param>
        /// <param name="nLenght"></param>
        public void CreateParameter(string strParameterName, string strParameter, int nLenght)
        {
            //Si la longitud es Cero ponerlo como Text
            if (nLenght == 0)
            {
                command.Parameters.Add(strParameterName, System.Data.SqlDbType.NText).Value = strParameter;
            }
            else
            {
                command.Parameters.Add(strParameterName, System.Data.SqlDbType.NVarChar, nLenght).Value = strParameter;
            }
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Fecha
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="dateTimeParameter"></param>
        public void CreateParameter(string strParameterName, DateTime dateTimeParameter)
        {
            command.Parameters.Add(strParameterName, System.Data.SqlDbType.DateTime, 8).Value = dateTimeParameter;
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Flotante
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="fParameter"></param>
        public void CreateParameter(string strParameterName, float fParameter)
        {
            command.Parameters.Add(strParameterName, System.Data.SqlDbType.Float, 8).Value = fParameter;
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Lógico
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="bParameter"></param>
        public void CreateParameter(string strParameterName, bool bParameter)
        {
            command.Parameters.Add(strParameterName, System.Data.SqlDbType.Bit, 1).Value = bParameter;
        }

        /// <summary>
        /// Crea Parametro de entrada de tipo Imagen
        /// </summary>
        /// <param name="strParameterName"></param>
        /// <param name="imgParameter"></param>
        public void CreateParameter(string strParameterName, byte[] imgParameter)
        {
            command.Parameters.Add(strParameterName, SqlDbType.Image).Value = imgParameter;
        }

        /// <summary>
        /// Ejecuta comando
        /// </summary>
        public void ExecuteNonQuery()
        {
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Ejecuta Escalar
        /// </summary>
        public object getScalar()
        {
            if (con.State == ConnectionState.Closed)
            { con.Open(); }
            return command.ExecuteScalar();
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="sql">Name of stored procedure.</param>
        /// <returns>Stored procedure return value.</returns>
        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 36000;
                this.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al ejecutar getDataTable. Error = [" + ex.Message + "]");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        /// <summary>
        /// Regresa Reader
        /// </summary>
        /// <param name="dataReader">Return result of procedure.</param>
        public SqlDataReader getDataReader()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al ejecutar getDataReader. Error = [" + ex.Message + "]");
            }
            //finally
            //{
            //    if (con.State == ConnectionState.Open)
            //        con.Close();
            //}
        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="sql">Name of stored procedure.</param>
        /// <param name="dataReader">Return result of procedure.</param>
        public SqlDataReader getDataReader(string sql)
        {
            try
            {
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 36000;
                this.Open();
                dr = cmd.ExecuteReader(); //System.Data.CommandBehavior.CloseConnection);
                cmd.Dispose();
                this.Close();
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al ejecutar getDataTable. Error = [" + ex.Message + "]");
            }
            //finally {
            //    if (con.State == ConnectionState.Open)
            //        con.Close();
            //}
        }


        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="sql">Name of stored procedure.</param>
        /// <param name="DataTable">Return result of procedure.</param>
        public DataTable getDataTable(string sql)
        {
            try
            {
                this.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                if (da.SelectCommand != null)
                { da.SelectCommand.CommandTimeout = 3600; }
                int rows = da.Fill(dt);
                dt.Dispose();
                this.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al ejecutar getDataTable. Error = [" + ex.Message + "]");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

        }

        /// <summary>
        /// Run stored procedure.
        /// </summary>
        /// <param name="sql">Name of stored procedure.</param>
        /// <param name="DataSet">Return result of procedure.</param>
        public DataSet getDataSet(string sql)
        {
            try
            {
                this.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataSet ds = new DataSet();
                if (da.SelectCommand != null)
                { da.SelectCommand.CommandTimeout = 3600; }
                da.Fill(ds);
                ds.Dispose();
                this.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al ejecutar getDataSet. Error = [" + ex.Message + "]");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        /// <summary>
        /// Regresa DataSet
        /// </summary>
        /// <param name="DataSet">Return result of procedure.</param>
        public DataSet getDataSet()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                SqlDataAdapter dataAdapterSearch = new SqlDataAdapter();
                DataSet data = new DataSet();
                dataAdapterSearch.SelectCommand = command;
                dataAdapterSearch.Fill(data);
                dataAdapterSearch.Dispose();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al ejecutar getDataSet. Error = [" + ex.Message + "]");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        public string obtenerSentenciaSQL() /*Procedimiento que te permite obtener la sentencia SQL 
            que se ejecutará (ideal para hacer pruebas) (Luis Enrique Mares Ortega [15/01/2014])*/
        {
            string sentenciaSQL = "EXEC " + command.CommandText;
            for (int numeroParametro = 0; numeroParametro < command.Parameters.Count; numeroParametro++)
            {
                if (numeroParametro != command.Parameters.Count && numeroParametro > 0)
                    sentenciaSQL += ',';

                if (command.Parameters[numeroParametro].DbType.ToString().Contains("String"))
                    sentenciaSQL += " " + command.Parameters[numeroParametro].ToString() + "=" + "'" + command.Parameters[numeroParametro].Value + "'";
                else
                    sentenciaSQL += " " + command.Parameters[numeroParametro].ToString() + "=" + command.Parameters[numeroParametro].Value;
            }
            return sentenciaSQL;
        }
        public int getValue(string sql)
        {
            try
            {
                this.Open();
                SqlCommand cmd = new SqlCommand();
                int returnValue;

                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                con.Open();

                returnValue = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion al ejecutar getValue. Error = [" + ex.Message + "]");
            }
            /*finally
            {
                if (sqlConnection1.State == ConnectionState.Open)
                    sqlConnection1.Close();
            }*/

        }
    }
}
