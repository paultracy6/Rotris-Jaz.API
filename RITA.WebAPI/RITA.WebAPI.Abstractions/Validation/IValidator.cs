namespace RITA.WebAPI.Abstractions.Validation
{
    public interface IValidator<in T>
    {
        void ValidateInsert(T model);
        void ValidateUpdate(T model);
    }
}
