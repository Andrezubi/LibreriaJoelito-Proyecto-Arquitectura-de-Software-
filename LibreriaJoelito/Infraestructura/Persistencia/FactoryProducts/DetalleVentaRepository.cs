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
            string query = @"SELECT dv.IdVenta AS IdVenta, dv.IdProducto AS IdProducto, dv.IdPresentacion AS IdPresentacion, 
                                    dv.Cantidad AS Cantidad, dv.PrecioUnitario AS PrecioUnitario, dv.Subtotal AS Subtotal, 
                                    pp.FactorConversion AS FactorConversion FROM detalleventa dv
                             INNER JOIN presentacionproducto pp ON dv.IdPresentacion = pp.IdPresentacion AND dv.IdProducto = pp.IdProducto
                             WHERE dv.IdVenta = @idVenta";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@idVenta", idVenta);

            return RepositorioBD.Instancia.ExecuteReturningDataTable(command);
        }
        public DataTable GetVentaDetalladaById(int idVenta)
        {
            string query = @"SELECT 
                                v.Fecha,
                                c.Ci,
                                c.Nombre AS ClienteNombre,
                                c.ApellidoPaterno,
                                v.Total,

                                CONCAT(u.Nombre, ' ', u.ApellidoPaterno) AS NombreEmpleado,

                                dv.Cantidad,

                                CONCAT(pr.Nombre,' de ',p.Nombre, ' ', m.Nombre) AS DescripcionProducto,

                                dv.PrecioUnitario,
                                dv.Subtotal

                            FROM Venta v

                            INNER JOIN Cliente c 
                                ON v.IdCliente = c.Id

                            INNER JOIN Usuario u 
                                ON v.IdUsuario = u.Id

                            INNER JOIN DetalleVenta dv 
                                ON v.Id = dv.IdVenta

                            INNER JOIN Producto p 
                                ON dv.IdProducto = p.Id

                            INNER JOIN Marca m 
                                ON p.IdMarca = m.Id  

                            INNER JOIN Presentacion pr 
                                ON dv.IdPresentacion = pr.Id

                            WHERE v.Id = @idVenta
                              AND v.Estado = TRUE;";
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
