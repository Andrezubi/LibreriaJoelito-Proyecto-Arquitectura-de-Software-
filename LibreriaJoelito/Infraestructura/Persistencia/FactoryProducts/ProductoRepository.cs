using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class ProductoRepository : RepositorioBD, IRepository<Producto>
    {
        public int Delete(Producto producto)
        {
            string query = @"UPDATE producto
                     SET Estado = 0, FechaUltimaActualizacion=@fechaAhora, IdUsuario=@idUsuario
                     WHERE Id = @Id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            cmd.Parameters.AddWithValue("@idUsuario", producto.IdUsuario);
            cmd.Parameters.AddWithValue("@Id", producto.Id);

            return ExecuteNonQuery(cmd);
            
        }

        public DataTable GetAll()
        {
            string query = @"SELECT  Id, Nombre,IdCategoria,IdMarca,Stock,Estado,FechaRegistro,IdUsuario,FechaUltimaActualizacion
                            FROM producto
                            WHERE Estado=1
                            ORDER BY 3";
            MySqlCommand command = new MySqlCommand(query);

            return ExecuteReturningDataTable(command);
        }

        public DataRow GetById(int id)
        {
            string query = @"SELECT  Id, Nombre,IdCategoria,IdMarca,Stock,Estado,FechaRegistro,IdUsuario,FechaUltimaActualizacion
                            FROM producto
                            WHERE Estado=1 and Id=@id
                            ORDER BY 3";
            
            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@id", id);

            return ExecuteReturningDataRow(command);
        }

        public int Insert(Producto producto)
        {
            string query = @"INSERT INTO producto ( Nombre,IdCategoria,IdMarca,Stock,FechaRegistro,IdUsuario)
                            VALUES (@nombre,@idCategoria,@idMarca,@stock,@fechaRegistro,@idUsuario)";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@nombre", producto.Nombre);
            command.Parameters.AddWithValue("@idCategoria", producto.IdCategoria);
            command.Parameters.AddWithValue("@idMarca", producto.IdMarca);
            command.Parameters.AddWithValue("@stock", producto.Stock);
            command.Parameters.AddWithValue("@fechaRegistro",producto.FechaRegistro);
            command.Parameters.AddWithValue("@idUsuario", producto.IdUsuario);
            return ExecuteNonQuery(command);
        }

        public int Update(Producto producto)
        {
            string query = @"UPDATE bdlibreria.producto
                            SET IdCategoria = @idCategoria,
	                            Nombre = @nombre,
                                IdMarca = @idMarca,
                                Stock = @stock,
                                FechaUltimaActualizacion = @fechaAhora,
                                IdUsuario=@idUsuario
                                
                            WHERE Id = @id;";

            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idCategoria",producto.IdCategoria);
            command.Parameters.AddWithValue("@nombre", producto.Nombre);
            command.Parameters.AddWithValue("@idMarca", producto.IdMarca);
            command.Parameters.AddWithValue("@stock", producto.Stock);
            command.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            command.Parameters.AddWithValue("@id", producto.Id);
            command.Parameters.AddWithValue("@idUsuario",producto.IdUsuario);
            return ExecuteNonQuery(command);
        }

        public bool ExisteDuplicado(Producto producto)
        {
            return false;
        }
        public DataTable BuscarPorNombre(string frase)
        {
            frase = frase.ToLower();
            string query = @"SELECT Nombre
                    FROM producto 
                    WHERE Estado = 1 AND Nombre LIKE @frase 
                    ORDER BY Nombre ASC 
                    LIMIT 10";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@frase", "%" + frase + "%");

            return ExecuteReturningDataTable(command);
        }
    }
}
