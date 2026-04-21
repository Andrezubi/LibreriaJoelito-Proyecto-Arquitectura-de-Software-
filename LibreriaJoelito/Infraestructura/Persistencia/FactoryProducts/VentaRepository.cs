using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class VentaRepository : IVentaRepository, IRepository<Venta>
    {
        public int Insert(Venta venta)
        {
            string query = @"INSERT INTO venta (IdCliente,Total,IdUsuario)
                             VALUES (@idCliente,@total,@idUsuario);
                             SELECT LAST_INSERT_ID();";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idCliente", venta.IdCliente);
            command.Parameters.AddWithValue("@total", venta.Total);
            command.Parameters.AddWithValue("@idUsuario", venta.IdUsuario);

            return Convert.ToInt32(RepositorioBD.Instancia.ExecuteScalar(command));
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

            return RepositorioBD.Instancia.ExecuteNonQuery(command);
        }

        public DataTable GetAll()
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, IdUsuario
                            FROM venta
                            WHERE Estado=1
                            ORDER BY 3";
            MySqlCommand command = new MySqlCommand(query);

            return RepositorioBD.Instancia.ExecuteReturningDataTable(command);
        }

        public DataRow GetById(int id)
        {
            string query = @"SELECT  Id, IdCliente, Fecha, Total, FechaRegistro, FechaUltimaActualizacion, IdUsuario
                            FROM venta
                            WHERE Estado=1 and Id=@id
                            ORDER BY 3";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@id", id);

            return RepositorioBD.Instancia.ExecuteReturningDataRow(command);
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

            return RepositorioBD.Instancia.ExecuteReturningDataTable(command);
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

            return RepositorioBD.Instancia.ExecuteReturningDataTable(command);
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

            return RepositorioBD.Instancia.ExecuteNonQuery(command);
        }

        public bool ExisteDuplicado(Venta venta)
        {
            return false;
        }

        public DataTable ObtenerDatosComprobante(int idVenta)
        {
            string query = @"
                SELECT 
                    v.Id AS VentaId, 
                    v.Fecha, 
                    v.Total,
                    v.FechaRegistro,
                    c.Ci, 
                    c.Complemento, 
                    c.Nombre AS ClienteNombre, 
                    c.ApellidoPaterno, 
                    c.ApellidoMaterno,
                    u.Username AS NombreEmpleado,
                    dv.Cantidad, 
                    CONCAT(pr.Nombre, ' de ', p.Nombre, ' ', m.Nombre) AS DescripcionProducto,
                    dv.PrecioUnitario, 
                    dv.Subtotal
                FROM venta v
                INNER JOIN cliente c ON v.IdCliente = c.Id
                INNER JOIN usuario u ON v.IdUsuario = u.Id
                INNER JOIN detalleventa dv ON v.Id = dv.IdVenta
                INNER JOIN producto p ON dv.IdProducto = p.Id
                INNER JOIN marca m ON p.IdMarca = m.Id
                INNER JOIN presentacion pr ON dv.IdPresentacion = pr.Id
                WHERE v.Id = @idVenta AND v.Estado = 1";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@idVenta", idVenta);

            return RepositorioBD.Instancia.ExecuteReturningDataTable(command);
        }



        public DataTable LoadVentas()
        {
            string query = @"
                SELECT v.Id,
                       v.Estado AS EstadoVenta,
                       c.Ci AS CiCliente,
                       c.Nombre AS NombreCliente,
                       v.Fecha
                FROM venta v
                INNER JOIN cliente c ON v.IdCliente = c.Id
                INNER JOIN usuario u ON v.IdUsuario = u.Id
                ORDER BY v.Fecha DESC";

            MySqlCommand cmd = new MySqlCommand(query);

            return RepositorioBD.Instancia.ExecuteReturningDataTable(cmd);
        }
    }
}
