namespace RITA.WebAPI.Abstractions.Repository
{
    public interface IBaseRepository<T>
    {
        T GetById(int id);

        T Insert(T entity);

        T Update(T entity);

        int Delete(int id);
    }
}
