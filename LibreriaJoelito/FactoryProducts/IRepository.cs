using System.Data;

namespace LibreriaJoelito.FactoryProducts
{
    public interface IRepository<T>
    {
        int Insert(T t);
        int Update(T t);
        int Delete(T t);
        DataTable GetAll();
<<<<<<< HEAD
        DataRow GetByID(int id);
=======
        DataRow GetById(int id);
>>>>>>> empleados-crud-camila
    }
}
