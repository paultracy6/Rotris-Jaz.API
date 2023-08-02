using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Validation;

namespace RITA.WebAPI.Abstractions.Services.Validation
{
    public interface IFavoriteValidator
    {
        IList<IInvalidField> ValidateInsert(IFavoriteModel model);
    }
}
