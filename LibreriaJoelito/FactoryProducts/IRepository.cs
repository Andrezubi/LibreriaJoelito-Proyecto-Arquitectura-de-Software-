using System.Data;

namespace LibreriaJoelito.FactoryProducts
{
    public interface IRepository<T>
    {
        int Insert(T t);
        int Update(T t, int id);
        int Delete(int id);
        DataTable GetAll();
        DataRow GetById(int id);
    }
}
