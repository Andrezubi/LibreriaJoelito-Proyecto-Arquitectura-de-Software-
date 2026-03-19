using LibreriaJoelito.Models;
using System.Data;

namespace LibreriaJoelito.FactoryProducts
{
    public interface IRepository<T>
    {
        int Insert(T t);
        int Update(T t);
        int Delete(T t);
        DataTable GetAll();
        DataRow GetById(int id);
        bool ExisteDuplicado(T t);
    }
}
