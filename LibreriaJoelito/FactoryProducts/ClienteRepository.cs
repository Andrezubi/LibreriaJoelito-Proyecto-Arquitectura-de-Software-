using System;
using System.Data;
using LibreriaJoelito.Models;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.FactoryProducts
{
    public class RepositoryClientes : IRepository<Cliente>
    {
        public int Insert(Cliente c)
        {
            string query = @"INSERT INTO Cliente 
                             (Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, Email, ClienteFrecuente) 
                             VALUES (@nombre, @paterno, @materno, @ci, @complemento, @email, @frecuente)";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@nombre", c.Nombre);
            cmd.Parameters.AddWithValue("@paterno", c.ApellidoPaterno);
            cmd.Parameters.AddWithValue("@materno", string.IsNullOrWhiteSpace(c.ApellidoMaterno) ? (object)DBNull.Value : c.ApellidoMaterno);
            cmd.Parameters.AddWithValue("@ci", c.CI);
            cmd.Parameters.AddWithValue("@complemento", string.IsNullOrWhiteSpace(c.Complemento) ? (object)DBNull.Value : c.Complemento);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(c.Email) ? (object)DBNull.Value : c.Email);
            cmd.Parameters.AddWithValue("@frecuente", c.EsClienteFrecuente);

            return RepositorioBD.ExecuteNonQuery(cmd);
        }

        public int Update(Cliente c)
        {
            string query = @"UPDATE Cliente 
                             SET Nombre = @nombre, ApellidoPaterno = @paterno, ApellidoMaterno = @materno, 
                                 Ci = @ci, Complemento = @complemento, Email = @email, ClienteFrecuente = @frecuente,
                                 Estado = @estado
                             WHERE Id = @id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@id", c.Id);
            cmd.Parameters.AddWithValue("@nombre", c.Nombre);
            cmd.Parameters.AddWithValue("@paterno", c.ApellidoPaterno);
            cmd.Parameters.AddWithValue("@materno", string.IsNullOrWhiteSpace(c.ApellidoMaterno) ? (object)DBNull.Value : c.ApellidoMaterno);
            cmd.Parameters.AddWithValue("@ci", c.CI);
            cmd.Parameters.AddWithValue("@complemento", string.IsNullOrWhiteSpace(c.Complemento) ? (object)DBNull.Value : c.Complemento);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(c.Email) ? (object)DBNull.Value : c.Email);
            cmd.Parameters.AddWithValue("@frecuente", c.EsClienteFrecuente);
            cmd.Parameters.AddWithValue("@estado", c.Estado);

            return RepositorioBD.ExecuteNonQuery(cmd);
        }

        public int Delete(Cliente c)
        {
            // Borrado lógico (se asume que la vista/controlador pasa el c.Id)
            string query = @"UPDATE Cliente SET Estado = 0 WHERE Id = @id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@id", c.Id);
            return RepositorioBD.ExecuteNonQuery(cmd);
        }

        public DataTable GetAll()
        {
            string query = @"SELECT Id, Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, Email, ClienteFrecuente, Estado, FechaRegistro 
                             FROM Cliente WHERE Estado = 1";
            MySqlCommand cmd = new MySqlCommand(query);
            return RepositorioBD.ExecuteReturningDataTable(cmd);
        }
    }
}
