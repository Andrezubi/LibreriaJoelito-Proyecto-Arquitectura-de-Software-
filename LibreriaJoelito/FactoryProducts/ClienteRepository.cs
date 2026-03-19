using LibreriaJoelito.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace LibreriaJoelito.FactoryProducts
{
    public class ClienteRepository : IRepository<Cliente>
    {
        public int Delete(Cliente t)
        {
            MySqlCommand cmd = new MySqlCommand(@"
                UPDATE Cliente SET
                    Estado                   = 0,
                    FechaUltimaActualizacion = NOW()
                WHERE Id = @id");

            cmd.Parameters.AddWithValue("@id", t.Id);
            return RepositorioBD.ExecuteNonQuery(cmd);
        }

        public DataTable GetAll()
        {
            MySqlCommand cmd = new MySqlCommand(@"
                SELECT Id, Nombre, ApellidoPaterno, ApellidoMaterno,
                       Ci AS CI, Complemento, Email, ClienteFrecuente AS ClienteFrecuente, FechaRegistro
                FROM Cliente
                WHERE Estado = 1
                ORDER BY ApellidoPaterno, Nombre");

            return RepositorioBD.ExecuteReturningDataTable(cmd);
        }

        public DataRow GetById(int id)
        {
            MySqlCommand cmd = new MySqlCommand(@"
                SELECT Id, Nombre, ApellidoPaterno, ApellidoMaterno,
                       Ci AS Ci, Complemento, Email, ClienteFrecuente AS ClienteFrecuente, FechaRegistro
                FROM Cliente
                WHERE Id = @id AND Estado = 1");

            cmd.Parameters.AddWithValue("@id", id);

            return RepositorioBD.ExecuteReturningDataRow(cmd);
        }

        public int Insert(Cliente t)
        {
            MySqlCommand cmd = new MySqlCommand(@"
                INSERT INTO Cliente 
                    (Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, Email, ClienteFrecuente)
                VALUES 
                    (@nombre, @apellidoPaterno, @apellidoMaterno, @ci, @complemento, @email, @clienteFrecuente)");

            AgregarParametros(cmd, t);
            return RepositorioBD.ExecuteNonQuery(cmd);

        }

        public int Update(Cliente t)
        {
            MySqlCommand cmd = new MySqlCommand(@"
                UPDATE Cliente SET
                    Nombre                   = @nombre,
                    ApellidoPaterno          = @apellidoPaterno,
                    ApellidoMaterno          = @apellidoMaterno,
                    Ci                       = @ci,
                    Complemento              = @complemento,
                    Email                    = @email,
                    ClienteFrecuente         = @clienteFrecuente,
                    FechaUltimaActualizacion = NOW()
                WHERE Id = @id");

            AgregarParametros(cmd, t);
            cmd.Parameters.AddWithValue("@id", t.Id);
            return RepositorioBD.ExecuteNonQuery(cmd);
        }

        public bool ExisteDuplicado(Cliente cliente)
        {
            MySqlCommand cmd = new MySqlCommand(@"
                SELECT COUNT(*) FROM Cliente
                WHERE Ci          = @ci
                  AND Complemento = @complemento
                  AND Id         <> @id
                  AND Estado      = 1");

            cmd.Parameters.AddWithValue("@ci", cliente.Ci);
            cmd.Parameters.AddWithValue("@complemento", cliente.Complemento ?? string.Empty);
            cmd.Parameters.AddWithValue("@id", cliente.Id);

            return Convert.ToInt32(RepositorioBD.ExecuteScalar(cmd)) > 0;
        }

        // --- Métodos privados de apoyo ---

        static void AgregarParametros(MySqlCommand cmd, Cliente cliente)
        {
            cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
            cmd.Parameters.AddWithValue("@apellidoPaterno", cliente.ApellidoPaterno);
            cmd.Parameters.AddWithValue("@apellidoMaterno", (object?)cliente.ApellidoMaterno ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ci", cliente.Ci);
            cmd.Parameters.AddWithValue("@complemento", cliente.Complemento ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", (object?)cliente.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@clienteFrecuente", cliente.ClienteFrecuente);
        }

    }
}
