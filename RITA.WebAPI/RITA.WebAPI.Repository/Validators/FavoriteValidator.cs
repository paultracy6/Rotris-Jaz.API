using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Converters;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Utility;

namespace RITA.WebAPI.Repository.Validators
{
    public class FavoriteValidator : IFavoriteModelValidator
    {
        private readonly IRepositoryValidator _validator;

        public FavoriteValidator(IRepositoryValidator validator)
        {
            _validator = validator;
        }
        public void ValidateInsert(IFavoriteModel model)
        {
            var fields = new List<IInvalidField>();

            if (model.Id != 0)
                fields.Add(new InvalidField("Id", "must equal zero"));

            validateRequiredFields(model, fields);

            if (fields.Count > 0)
                throw new ValidationException("Favorite Insertion Error", fields);
        }

        internal static void validateRequiredFields(IFavoriteModel model, List<IInvalidField> fields)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                fields.Add(new InvalidField("Name", $"must contain a value with at least one character"));
            else
            {
                var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToFavorite(), "Name");
                if (!isValid)
                    fields.Add(new InvalidField("Name", $"exceeds Max Length allowed {maxLength}"));
            }

            if (string.IsNullOrWhiteSpace(model.UserId))
                fields.Add(new InvalidField("UserId", $"must contain a value with at least one character"));
            else
            {
                var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToFavorite(), "UserId");
                if (!isValid)
                    fields.Add(new InvalidField("UserId", $"exceeds Max Length allowed {maxLength}"));
            }

            if (string.IsNullOrWhiteSpace(model.FavoriteType))
                fields.Add(new InvalidField("FavoriteType", $"must contain a value with at least one character"));
            else
            {
                var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToFavorite(), "FavoriteType");
                if (!isValid)
                    fields.Add(new InvalidField("FavoriteType", $"exceeds Max Length allowed {maxLength}"));
            }

            if (model.ReferenceId <= 0)
                fields.Add(new InvalidField("ReferenceId", "must be greater than zero"));
        }
    }
}
