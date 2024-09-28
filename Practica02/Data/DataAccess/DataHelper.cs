using System.Data;
using System.Data.SqlClient;

namespace Practica03.Data.DataAccess
{
    public class DataHelper
    {
        private static DataHelper _instance;
        private SqlConnection _connection;
        private string cnnString;

        private DataHelper()
        {
            cnnString = "server = DESKTOP-AR80EN9; database = Practica03; Integrated Security=True; Encrypt=False";
            _connection = new SqlConnection(cnnString);
        }

        public SqlConnection GetConection()
        {
            return _connection;
        }

        public static DataHelper GetInstance()
        {
            if( _instance == null )
            {
                _instance = new DataHelper();
            }
            return _instance;
        }

        public DataTable ExecuteSPQuery(string sp, List<SQLParameter>? parametros)
        {
            DataTable tabla = new DataTable();
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                using (var cmd = new SqlCommand(sp, _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parametros != null)
                    {
                        foreach (var param in parametros)
                        {
                            cmd.Parameters.AddWithValue(param.Name, param.Value);
                        }
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        tabla.Load(reader);
                    }
                }
            }
            catch (SqlException ex)  
            {
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return null;  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                return null;  
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }

            return tabla;
        }




        public bool ExecuteCRUDSPQuery(string sp, List<SQLParameter>? parametros)
        {
            bool returnQuery;
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                var cmd = new SqlCommand(sp, _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (parametros != null)
                {
                    foreach (var param in parametros)
                    cmd.Parameters.AddWithValue(param.Name, param.Value);
                }
                if(cmd.ExecuteNonQuery() != 0)
                {
                    returnQuery = true;
                }
                else
                {
                    returnQuery = false;
                }
            }
            catch (SqlException)
            {
                returnQuery = false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return returnQuery;
        }






        public bool ExecuteCRUDSPQueryWithTransaction(string sp, List<SQLParameter>? parametros, SqlTransaction transaction)
        {
            bool returnQuery = false;
            try
            {
                var cmd = new SqlCommand(sp, transaction.Connection, transaction);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var param in parametros)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }

                if (cmd.ExecuteNonQuery() != 0)
                {
                    returnQuery = true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception: {ex.Message}");
                returnQuery = false;
            }
            return returnQuery;
        }

    }
}
