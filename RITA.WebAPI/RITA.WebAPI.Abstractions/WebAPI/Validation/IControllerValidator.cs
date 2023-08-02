namespace RITA.WebAPI.Abstractions.WebAPI.Validation
{
    public interface IControllerValidator<in T>
    {
        void ValidateInsert(T view);
        void ValidateUpdate(int id, T view);
    }
}
