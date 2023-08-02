using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Business.Models;

namespace RITA.WebAPI.Business.Validators
{
    public class FavoriteValidator : IFavoriteValidator
    {
        public IList<IInvalidField> ValidateInsert(IFavoriteModel model)
        {
            var fields = new List<IInvalidField>();

            if (model.Id != 0)
                fields.Add(new InvalidField("Id", "must be equal zero"));

            if (model.ReferenceId < 1)
                fields.Add(new InvalidField("ReferenceId", "must be greater than zero"));

            if (string.IsNullOrWhiteSpace(model.UserId))
                fields.Add(new InvalidField("UserId", "cannot be empty"));

            if (string.IsNullOrWhiteSpace(model.Name))
                fields.Add(new InvalidField("Name", "cannot be empty"));

            if (string.IsNullOrWhiteSpace(model.FavoriteType))
                fields.Add(new InvalidField("FavoriteType", "cannot be empty"));

            if (fields.Count > 0)
                throw new ValidationException("Favorite Insertion Error", fields);

            return fields;
        }
    }
}
