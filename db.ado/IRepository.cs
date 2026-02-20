using Models;

namespace Ado.Db;

public interface IRepository<T> where T : EntityBase
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}
