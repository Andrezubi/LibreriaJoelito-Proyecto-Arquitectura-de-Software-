using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class VentaRepository : IVentaRepository
    {
        public int Insert(Venta venta)
        {
            string query = @"INSERT INTO venta (IdCliente,Total,IdUsuario)
                             VALUES (@idCliente,@total,@idUsuario);";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idCliente", venta.IdCliente);
            command.Parameters.AddWithValue("@total", venta.Total);
            command.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);

            return RepositorioBD.ExecuteNonQuery(command);
        }

        public int Delete(Venta venta)
        {
            string query = @"UPDATE venta
                             SET Estado = 0, FechaUltimaActualizacion=@fechaAhora
                             WHERE Id = @Id";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            command.Parameters.AddWithValue("@Id", venta.Id);

            return RepositorioBD.ExecuteNonQuery(command);
        }

        public DataTable GetAll()
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, IdUsuario
                            FROM venta
                            WHERE Estado=1
                            ORDER BY 3";
            MySqlCommand command = new MySqlCommand(query);

            return RepositorioBD.ExecuteReturningDataTable(command);
        }

        public DataRow GetById(int id)
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, FechaUltimaActualizacion, IdUsuario
                            FROM venta
                            WHERE Estado=1 and Id=@id
                            ORDER BY 3";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@id", id);

            return RepositorioBD.ExecuteReturningDataRow(command);
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

            return RepositorioBD.ExecuteReturningDataTable(command);
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

            return RepositorioBD.ExecuteReturningDataTable(command);
        }
    }
}
