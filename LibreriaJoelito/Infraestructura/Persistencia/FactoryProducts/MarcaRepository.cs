using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class MarcaRepository : RepositorioBD, IRepository<Marca>
    {
        public int Delete(Marca marca)
        {
            string query = @"UPDATE Marca 
                             SET Estado = 0, FechaUltimaActualizacion = @fecha 
                             WHERE Id = @id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", marca.Id);
            return ExecuteNonQuery(cmd);
        }

        public DataTable GetAll()
        {
            string query = @"SELECT Id, Nombre, Descripcion, PaginaWeb, Industria, Estado, FechaRegistro, IdUsuario 
                             FROM Marca 
                             WHERE Estado = 1 
                             ORDER BY Nombre";
            MySqlCommand cmd = new MySqlCommand(query);
            return ExecuteReturningDataTable(cmd);
        }

        public DataRow GetById(int id)
        {
            string query = @"SELECT Id, Nombre, Descripcion, PaginaWeb, Industria, Estado, FechaRegistro, IdUsuario 
                             FROM Marca 
                             WHERE Id = @id AND Estado = 1";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@id", id);
            return ExecuteReturningDataRow(cmd);
        }

        public int Insert(Marca marca)
        {
            string query = @"INSERT INTO Marca (Nombre, Descripcion, PaginaWeb, Industria, IdUsuario) 
                             VALUES (@nombre, @desc, @web, @ind, @idU)";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@nombre", marca.Nombre);
            cmd.Parameters.AddWithValue("@desc", marca.Descripcion);
            cmd.Parameters.AddWithValue("@web", marca.PaginaWeb);
            cmd.Parameters.AddWithValue("@ind", marca.Industria);
            cmd.Parameters.AddWithValue("@idU", marca.IdUsuario);
            return ExecuteNonQuery(cmd);
        }

        public int Update(Marca marca)
        {
            string query = @"UPDATE Marca 
                             SET Nombre = @nombre, 
                                 Descripcion = @desc, 
                                 PaginaWeb = @web, 
                                 Industria = @ind, 
                                 FechaUltimaActualizacion = @fecha 
                             WHERE Id = @id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@nombre", marca.Nombre);
            cmd.Parameters.AddWithValue("@desc", marca.Descripcion);
            cmd.Parameters.AddWithValue("@web", marca.PaginaWeb);
            cmd.Parameters.AddWithValue("@ind", marca.Industria);
            cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", marca.Id);
            return ExecuteNonQuery(cmd);
        }

        public bool ExisteDuplicado(Marca marca)
        {
            string query = @"SELECT COUNT(*) 
                     FROM Marca 
                     WHERE UPPER(TRIM(Nombre)) = UPPER(TRIM(@nombre)) 
                       AND Id <> @id 
                       AND Estado = 1";

            MySqlCommand cmd = new MySqlCommand(query);

            cmd.Parameters.AddWithValue("@nombre", marca.Nombre);
            cmd.Parameters.AddWithValue("@id", marca.Id); 

            return Convert.ToInt32(ExecuteScalar(cmd)) > 0;
        }
    }
}