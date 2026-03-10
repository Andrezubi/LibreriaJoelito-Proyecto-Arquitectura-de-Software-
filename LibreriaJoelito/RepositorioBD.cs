using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace LibreriaJoelito
{
    public static class RepositorioBD
    {
        private static string connectionStringMySql = "Server=localhost;Port=3306;uid=root;pwd=root;database=bdlibreria";
        public static int ExecuteNonQuery(MySqlCommand comando)
        {
            using (MySqlConnection con = new MySqlConnection(connectionStringMySql))
            {
                con.Open();
                comando.Connection = con;
                return comando.ExecuteNonQuery();
            }
        }
        public static MySqlDataReader ExecuteReader(MySqlCommand comando)
        {
            MySqlConnection con = new MySqlConnection(connectionStringMySql);
            con.Open();
            comando.Connection = con;
            return comando.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public static MySqlDataAdapter ExecuteDataAdapter(MySqlCommand comando)
        {
            MySqlConnection con = new MySqlConnection(connectionStringMySql);
            comando.Connection = con;
            return new MySqlDataAdapter(comando);
        }
        public static DataTable ExecuteReturningDataTable(MySqlCommand comando)
        {
            using (MySqlConnection con = new MySqlConnection(connectionStringMySql))
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
    }
}
