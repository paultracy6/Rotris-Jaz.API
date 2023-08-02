using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Abstractions.Repository.Validation
{
    public interface IFavoriteModelValidator
    {
        void ValidateInsert(IFavoriteModel model);
    }
}
