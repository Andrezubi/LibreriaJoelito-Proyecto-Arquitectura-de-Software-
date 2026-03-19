using LibreriaJoelito.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.FactoryProducts
{
    public class ProductoRepository : IRepository<Producto>
    {
        public int Delete(Producto producto)
        {
            string query = @"UPDATE producto
                     SET Estado = 0, FechaUltimaActualizacion=@fechaAhora
                     WHERE Id = @Id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            cmd.Parameters.AddWithValue("@Id", producto.Id);

            return RepositorioBD.ExecuteNonQuery(cmd);
            
        }

        public DataTable GetAll()
        {
            string query = @"SELECT  Id, Nombre,IdCategoria,IdMarca,Stock,Estado,FechaRegistro,IdEmpleadoCambio,FechaUltimaActualizacion
                            FROM producto
                            WHERE Estado=1
                            ORDER BY 3";
            MySqlCommand command = new MySqlCommand(query);

            return RepositorioBD.ExecuteReturningDataTable(command);
        }

        public DataRow GetById(int id)
        {
            string query = @"SELECT  Id, Nombre,IdCategoria,IdMarca,Stock,Estado,FechaRegistro,IdEmpleadoCambio,FechaUltimaActualizacion
                            FROM producto
                            WHERE Estado=1 and Id=@id
                            ORDER BY 3";
            
            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@id", id);

            return RepositorioBD.ExecuteReturningDataRow(command);
        }

        public int Insert(Producto producto)
        {
            string query = @"INSERT INTO producto ( Nombre,IdCategoria,IdMarca,Stock,FechaRegistro)
                            VALUES (@nombre,@idCategoria,@idMarca,@stock,@fechaRegistro);";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@nombre", producto.Nombre);
            command.Parameters.AddWithValue("@idCategoria", producto.IdCategoria);
            command.Parameters.AddWithValue("@idMarca", producto.IdMarca);
            command.Parameters.AddWithValue("@stock", producto.Stock);
            command.Parameters.AddWithValue("@fechaRegistro",producto.FechaRegistro);
            return RepositorioBD.ExecuteNonQuery(command);
        }

        public int Update(Producto producto)
        {
            string query = @"UPDATE bdlibreria.producto
                            SET IdCategoria = @idCategoria,
	                            Nombre = @nombre,
                                IdMarca = @idMarca,
                                Stock = @stock,
                                FechaUltimaActualizacion = @fechaAhora
                                
                            WHERE Id = @id;";

            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idCategoria",producto.IdCategoria);
            command.Parameters.AddWithValue("@nombre", producto.Nombre);
            command.Parameters.AddWithValue("@idMarca", producto.IdMarca);
            command.Parameters.AddWithValue("@stock", producto.Stock);
            command.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            command.Parameters.AddWithValue("@id", producto.Id);
            return RepositorioBD.ExecuteNonQuery(command);
        }

        public bool ExisteDuplicado(Producto producto)
        {
            return false;
        }

    }
}
