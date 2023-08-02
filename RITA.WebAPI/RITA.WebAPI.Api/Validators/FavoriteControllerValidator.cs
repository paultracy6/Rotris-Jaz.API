using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Abstractions.WebAPI.Validation;
using RITA.WebAPI.Api.Models;

namespace RITA.WebAPI.Api.Validators
{
    public class FavoriteControllerValidator : IFavoriteControllerValidator
    {
        public void ValidateInsert(IFavoriteView view)
        {
            var fields = new List<IInvalidField>();

            if (view.Id != 0)
                fields.Add(new InvalidField("Id", "must be zero"));

            if (view.ReferenceId <= 0)
                fields.Add(new InvalidField("ReferenceId", "must be greater than zero"));

            if (fields.Count > 0)
                throw new ValidationException("Insertion Error", fields);
        }
    }
}
