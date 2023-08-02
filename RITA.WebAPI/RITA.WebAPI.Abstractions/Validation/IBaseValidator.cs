namespace RITA.WebAPI.Abstractions.Validation
{
    public interface IBaseValidator<T>
    {
        void Validate(T entity);
    }
}
