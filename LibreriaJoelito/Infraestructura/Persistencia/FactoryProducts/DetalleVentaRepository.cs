using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class DetalleVentaRepository : IDetalleVentaRepository, IRepository<DetalleVenta>
    {
        public int Insert(DetalleVenta detalleVenta)
        {
            string query = @"INSERT INTO detalleventa ( IdVenta, IdProducto, IdPresentacion, Cantidad, PrecioUnitario, Subtotal)
                            VALUES (@idVenta, @idProducto, @idPresentacion, @cantidad, @precioUnitario, @subtotal);";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idVenta", detalleVenta.IdVenta);
            command.Parameters.AddWithValue("@idProducto", detalleVenta.IdProducto);
            command.Parameters.AddWithValue("@idPresentacion", detalleVenta.IdPresentacion);
            command.Parameters.AddWithValue("@cantidad", detalleVenta.Cantidad);
            command.Parameters.AddWithValue("@precioUnitario", detalleVenta.PrecioUnitario);
            command.Parameters.AddWithValue("@subtotal", detalleVenta.Subtotal);
            return RepositorioBD.Instancia.ExecuteNonQuery(command);
        }

        public int Update(DetalleVenta detalleVenta)
        {
            return 0;
        }

        public DataRow GetById(int id)
        {
            return null;
        }

        public DataTable GetAll()
        {
            string query = @"SELECT * 
                             FROM detalleventa";
            MySqlCommand command = new MySqlCommand(query);

            return RepositorioBD.Instancia.ExecuteReturningDataTable(command);
        }

        public int Delete(DetalleVenta detalleVenta)
        {
            string query = @"DELETE FROM detalleventa
                             WHERE IdVenta = @idVenta
                                AND IdProducto = @idProducto
                                AND IdPresentacion = @idPresentacion";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idVenta", detalleVenta.IdVenta);
            command.Parameters.AddWithValue("@idProducto", detalleVenta.IdProducto);
            command.Parameters.AddWithValue("@idPresentacion", detalleVenta.IdPresentacion);

            return RepositorioBD.Instancia.ExecuteNonQuery(command);
        }

        public DataTable GetByIdVenta(int idVenta)
        {
            string query = @"SELECT IdVenta, IdProducto, IdPresentacion, Cantidad, PrecioUnitario, Subtotal FROM detalleventa
                             WHERE IdVenta = @idVenta";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idVenta", idVenta);

            return RepositorioBD.Instancia.ExecuteReturningDataTable(command);
        }

        public int DeleteByIdVenta(int idVenta)
        {
            string query = @"DELETE FROM detalleventa
                             WHERE IdVenta = @idVenta";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idVenta", idVenta);

            return RepositorioBD.Instancia.ExecuteNonQuery(command);
        }

        public bool ExisteDuplicado (DetalleVenta detalleVenta)
        {
            return false;
        }
    }
}
