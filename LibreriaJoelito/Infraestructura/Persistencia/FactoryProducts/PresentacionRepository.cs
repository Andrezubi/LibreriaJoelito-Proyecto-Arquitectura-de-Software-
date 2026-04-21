using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;

namespace LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts
{
    public class PresentacionRepository: RepositorioBD, IPresentacionRepository
    {
        public int Update(Dominio.Models.Presentacion t)
        {
            throw new NotImplementedException();
        }

        public int Insert(Dominio.Models.Presentacion t)
        {
            throw new NotImplementedException();
        }

        public int Delete(Dominio.Models.Presentacion t)
        {
            throw new NotImplementedException();
        }
        public bool ExisteDuplicado(Dominio.Models.Presentacion t)
        {
            throw new NotImplementedException();
        }
        public DataTable GetAll()
        {
            string query = "SELECT Id, Nombre FROM presentacion WHERE Estado = 1 ORDER BY Nombre";
            MySqlCommand cmd = new MySqlCommand(query);
            return ExecuteReturningDataTable(cmd);
        }
        public DataRow? GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
