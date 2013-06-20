using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace TRAWebServer
{
    class DBManager
    {
        private SqlConnection connection;
        private string connString;

        public SqlConnection Connection
        {
            get { return connection; }
        }

        public DBManager(string connectionString)
        {
            NewConnection(connectionString);
        }

        /// <summary>
        /// Creates a new SqlConnection. To retrieve connection use connection's properties
        /// </summary>
        /// <param name="connectionString"></param>
        private void NewConnection(string connectionString)
        {
            connString = connectionString;
            connection = new SqlConnection(connectionString);

            if (!IsConnectionOpen())
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL error. " + ex.ErrorCode + ":" + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Closes the DB connection
        /// </summary>
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Verifies the connection state
        /// </summary>
        /// <returns></returns>
        public bool IsConnectionOpen()
        {
            if (connection.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the query and gets a DataTable with the results of the specified query
        /// </summary>
        /// <param name="cmd">Sql Command</param>
        /// <returns>DataTable with Results</returns>
        public DataTable GetDataTableResults(SqlCommand cmd)
        {
            cmd.CommandTimeout = 180;

            DataTable results = new DataTable();

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(results);
                return results;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL error. " + ex.ErrorCode + ":" + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return results;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Executes a NonQuery 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool ExecuteNonQuery(SqlCommand cmd)
        {

            cmd.CommandTimeout = 180;
            
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL error. " + ex.ErrorCode + ":" + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// Executes a query and returns a DataReader
        /// </summary>
        /// <param name="cmd">SQL Command </param>
        /// <returns>SQL Data Reader with the Results</returns>
        /*public SqlDataReader GetDataReaderResults(SqlCommand cmd)
        {
            cmd.CommandTimeout = 180;

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                return dataReader;
            }
            catch
            {
            }
            finally
            {
                cmd.Connection.Close();
            }
        }*/

        public object ExecuteScalar(SqlCommand cmd)
        {
            object result = new object();

            try
            {
                result = cmd.ExecuteScalar();

                return result;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL error. " + ex.ErrorCode + ":" + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
}
