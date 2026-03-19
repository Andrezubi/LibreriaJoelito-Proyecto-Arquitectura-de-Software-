
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace LibreriaJoelito
{
    public static class RepositorioBD
    {

        private static string connectionStringMySql = "Server=localhost;Port=3306;uid=root;pwd=1234;database=bdlibreria";
        private static string? _connectionString;
        private static string CatchStringConnection()
        {
            return _connectionString
                ?? throw new InvalidOperationException("La cadena de conexión no ha sido configurada. Por favor, configure la cadena de conexión antes de usar el repositorio.");
        }

        public static void Initiate(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString", "La cadena de conexión no puede ser nula o vacía.");
            _connectionString = connectionString;
        }
        public static int ExecuteNonQuery(MySqlCommand comando)
        {
            using (MySqlConnection con = new MySqlConnection(CatchStringConnection()))
            {
                con.Open();
                comando.Connection = con;
                return comando.ExecuteNonQuery();
            }
        }
        public static MySqlDataReader ExecuteReader(MySqlCommand comando)
        {
            MySqlConnection con = new MySqlConnection(CatchStringConnection());
            con.Open();
            comando.Connection = con;
            return comando.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public static MySqlDataAdapter ExecuteDataAdapter(MySqlCommand comando)
        {
            MySqlConnection con = new MySqlConnection(CatchStringConnection());
            comando.Connection = con;
            return new MySqlDataAdapter(comando);
        }
        public static DataTable ExecuteReturningDataTable(MySqlCommand comando)
        {
            using (MySqlConnection con = new MySqlConnection(CatchStringConnection()))
            {
                con.Open();
                comando.Connection = con;

                using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(comando))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public static DataRow? ExecuteReturningDataRow(MySqlCommand comando)
        {
            DataTable dt = ExecuteReturningDataTable(comando);
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        public static object? ExecuteScalar(MySqlCommand comando)
        {
            using (MySqlConnection con = new MySqlConnection(CatchStringConnection()))
            {
                con.Open();
                comando.Connection = con;
                return comando.ExecuteScalar();
            }
        }
    }
}
