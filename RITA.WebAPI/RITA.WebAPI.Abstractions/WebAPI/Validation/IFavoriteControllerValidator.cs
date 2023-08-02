using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Abstractions.WebAPI.Validation
{
    public interface IFavoriteControllerValidator
    {
        void ValidateInsert(IFavoriteView view);
    }
}
