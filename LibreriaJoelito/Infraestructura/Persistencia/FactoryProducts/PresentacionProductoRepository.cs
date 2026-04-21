using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class PresentacionProductoRepository : RepositorioBD, IPresentacionProductoRepository
    {
        public int Delete(PresentacionProducto t)
        {
            throw new NotImplementedException();
        }

        public bool ExisteDuplicado(PresentacionProducto t)
        {
            throw new NotImplementedException();
        }

        public DataTable GetAll()
        {
            throw new NotImplementedException();
        }

        public DataRow? GetById(int id)
        {
            throw new NotImplementedException();
        }
        public DataRow? GetByIds(int idProducto, int idPresentacion)
        {
            string query = @"
                                SELECT 
                                    pp.IdProducto,
                                    pp.IdPresentacion,
                                    pp.Precio,
                                    pp.FactorConversion AS FactorConversion,
                                    p.Nombre AS Producto,
                                    pr.Nombre AS Presentacion,
                                    m.Nombre AS Marca,
                                    CONCAT(pr.Nombre, ' de ', p.Nombre, ' ', m.Nombre) AS Descripcion
                                FROM PresentacionProducto pp
                                INNER JOIN Producto p ON pp.IdProducto = p.Id
                                INNER JOIN Presentacion pr ON pp.IdPresentacion = pr.Id
                                LEFT JOIN Marca m ON p.IdMarca = m.Id
                                WHERE pp.IdProducto = @idProducto
                                  AND pp.IdPresentacion = @idPresentacion
                                  AND pp.Estado = 1
                                  AND p.Estado = 1
                                  AND pr.Estado = 1";

            var cmd = new MySqlCommand(query);

            cmd.Parameters.AddWithValue("@idProducto", idProducto);
            cmd.Parameters.AddWithValue("@idPresentacion", idPresentacion);

            var dt = ExecuteReturningDataTable(cmd);

            if (dt.Rows.Count > 0)
                return dt.Rows[0];

            return null;
        }

        public int Insert(PresentacionProducto t)
        {
            throw new NotImplementedException();
        }

        public DataTable obtenerPresentacionProductoDetallado(string frase)
        {
            string query = @"SELECT 
                                pp.IdProducto,
                                pp.IdPresentacion,
                                pp.Estado AS EstadoPresentacionProducto,
                                p.Nombre AS Producto,
                                pr.Nombre AS Presentacion,
                                m.Nombre AS Marca,
                                CONCAT(pr.Nombre, ' de ', p.Nombre, ' ', m.Nombre) AS Descripcion,
                                pp.Precio
                            FROM PresentacionProducto pp
                            INNER JOIN Producto p 
                                ON pp.IdProducto = p.Id
                            INNER JOIN Presentacion pr 
                                ON pp.IdPresentacion = pr.Id
                            LEFT JOIN Marca m 
                                ON p.IdMarca = m.Id
                            WHERE CONCAT(pr.Nombre, ' de ', p.Nombre, ' ', m.Nombre) 
                                  LIKE CONCAT('%', @frase, '%')
                              AND pp.Estado = TRUE
                              AND p.Estado = TRUE
                              AND pr.Estado = TRUE
                              AND (m.Estado = TRUE OR m.Id IS NULL);";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@frase", frase);
            return ExecuteReturningDataTable(cmd);

        }

        public int Update(PresentacionProducto t)
        {
            throw new NotImplementedException();
        }
    }
}
