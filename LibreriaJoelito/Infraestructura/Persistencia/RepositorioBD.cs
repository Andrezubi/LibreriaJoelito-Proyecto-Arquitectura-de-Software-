using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace LibreriaJoelito.Infraestructura.Persistencia
{
    public class RepositorioBD
    {
        private static string? _connectionString;
        private static readonly Lazy<RepositorioBD> _instancia = new Lazy<RepositorioBD>(() => new RepositorioBD());


        public static RepositorioBD Instancia
        {
            get
            {
                return _instancia.Value;
            }
        }

        private string CatchStringConnection()
        {
            return _connectionString
                ?? throw new InvalidOperationException("La cadena de conexión no ha sido configurada. Por favor, configure la cadena de conexión antes de usar el repositorio.");
        }

        public void Initiate(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString", "La cadena de conexión no puede ser nula o vacía.");
            _connectionString = connectionString;
        }

        public int ExecuteNonQuery(MySqlCommand comando)
        {
            using (MySqlConnection con = new MySqlConnection(CatchStringConnection()))
            {
                con.Open();
                comando.Connection = con;
                return comando.ExecuteNonQuery();
            }
        }
        public MySqlDataReader ExecuteReader(MySqlCommand comando)
        {
            MySqlConnection con = new MySqlConnection(CatchStringConnection());
            con.Open();
            comando.Connection = con;
            return comando.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public MySqlDataAdapter ExecuteDataAdapter(MySqlCommand comando)
        {
            MySqlConnection con = new MySqlConnection(CatchStringConnection());
            comando.Connection = con;
            return new MySqlDataAdapter(comando);
        }
        public DataTable ExecuteReturningDataTable(MySqlCommand comando)
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

        public DataRow? ExecuteReturningDataRow(MySqlCommand comando)
        {
            DataTable dt = ExecuteReturningDataTable(comando);
            if (dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        public object? ExecuteScalar(MySqlCommand comando)
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
