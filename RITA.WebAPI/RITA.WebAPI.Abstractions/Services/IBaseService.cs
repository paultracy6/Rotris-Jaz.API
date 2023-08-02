namespace RITA.WebAPI.Abstractions.Services
{
    public interface IBaseService<T>
    {
        T? GetById(int id);

        T Insert(T view, string createdBy);

        T Update(T view, string updatedBy);

        int Delete(int id);
    }
}
