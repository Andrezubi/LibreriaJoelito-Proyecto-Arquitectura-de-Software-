using LibreriaJoelito.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.FactoryProducts
{
    public class EmpleadoRepository : IRepository<Empleado>
    {
        public int Insert(Empleado t)
        {
            return 0;
        }
        public int Update(Empleado t)
        {
            string query = @"UPDATE empleado 
                     SET Nombre = @nombre, 
                         ApellidoPaterno = @apellidoPaterno, 
                         ApellidoMaterno = @apellidoMaterno, 
                         CI = @ci, 
                         Complemento = @complemento,
                         Email = @email, 
                         DireccionDomicilio = @direccion,
                         Telefono = @telefono,
                         FechaNacimiento = @fechaNacimiento,
                         FechaIngreso = @fechaIngreso,
                         FechaUltimaActualizacion = NOW() 
                     WHERE id = @id;";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@nombre", t.Nombre);
            command.Parameters.AddWithValue("@apellidoPaterno", t.ApellidoPaterno);
            command.Parameters.AddWithValue("@apellidoMaterno", t.ApellidoMaterno);
            command.Parameters.AddWithValue("@ci", t.Ci);
            command.Parameters.AddWithValue("@complemento", t.Complemento ?? "");
            command.Parameters.AddWithValue("@email", t.Email);
            command.Parameters.AddWithValue("@direccion", t.DireccionDomicilio);
            command.Parameters.AddWithValue("@telefono", t.Telefono);
            command.Parameters.AddWithValue("@fechaNacimiento", t.FechaNacimiento.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@fechaIngreso", t.FechaIngreso.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@id", t.Id);

            
            return RepositorioBD.ExecuteNonQuery(command);
        }
        public int Delete(Empleado t)
        {
            return 0;
        }
        public DataTable GetAll()
        {
            string query = @"SELECT Id,Nombre,ApellidoPaterno,ApellidoMaterno,Ci,Complemento,FechaNacimiento,Email,DireccionDomicilio,Telefono,FechaIngreso
                    FROM Empleado
                    WHERE estado = 1
                    ORDER BY 2;";
            MySqlCommand command = new MySqlCommand(query);
            return RepositorioBD.ExecuteReturningDataTable(command);
        }
        public DataRow GetById(int id)
        {
            return new DataTable().NewRow();
        }
    }
}
