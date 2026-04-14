using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class VentaRepository : RepositorioBD, IVentaRepository, IRepository<Venta>
    {
        public int Insert(Venta venta)
        {
            string query = @"INSERT INTO venta (IdCliente,Total,IdUsuario)
                             VALUES (@idCliente,@total,@idUsuario);";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idCliente", venta.IdCliente);
            command.Parameters.AddWithValue("@total", venta.Total);
            command.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);

            return ExecuteNonQuery(command);
        }

        public int Delete(Venta venta)
        {
            string query = @"UPDATE venta
                             SET Estado = 0, FechaUltimaActualizacion=@fechaAhora, IdUsuario=@idUsuario
                             WHERE Id = @Id";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            command.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);
            command.Parameters.AddWithValue("@Id", venta.Id);

            return ExecuteNonQuery(command);
        }

        public DataTable GetAll()
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, IdUsuario
                            FROM venta
                            WHERE Estado=1
                            ORDER BY 3";
            MySqlCommand command = new MySqlCommand(query);

            return ExecuteReturningDataTable(command);
        }

        public DataRow GetById(int id)
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, FechaUltimaActualizacion, IdUsuario
                            FROM venta
                            WHERE Estado=1 and Id=@id
                            ORDER BY 3";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@id", id);

            return ExecuteReturningDataRow(command);
        }

        public DataTable GetByDate(DateTime fechaInicio, DateTime fechaFin)
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, IdUsuario
                            FROM venta
                            WHERE Estado=1
                                AND Fecha BETWEEN @fechaInicio AND @fechaFin
                            ORDER BY 3";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            command.Parameters.AddWithValue("@fechaFin", fechaFin);

            return ExecuteReturningDataTable(command);
        }

        public DataTable GetByIdCliente(int idCliente)
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, IdUsuario
                            FROM venta
                            WHERE Estado=1 
                                AND IdCliente=@idCliente
                            ORDER BY 3";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idCliente", idCliente);

            return ExecuteReturningDataTable(command);
        }

        public int Update(Venta venta)
        {
            string query = @"UPDATE venta
                             SET IdCliente = @idCliente,
                                 Fecha = @fecha,
                                 Total = @total,
                                 FechaUltimaActualizacion=@fechaAhora,
                                 IdUsuario=@idUsuario
                             WHERE Id = @Id";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idCliente", venta.IdCliente);
            command.Parameters.AddWithValue("@fecha", venta.Fecha);
            command.Parameters.AddWithValue("@total", venta.Total);
            command.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);
            command.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            command.Parameters.AddWithValue("@Id", venta.Id);

            return ExecuteNonQuery(command);
        }

        public bool ExisteDuplicado(Venta venta)
        {
            return false;
        }
    }
}
