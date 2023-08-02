using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Converters;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Utility;

namespace RITA.WebAPI.Repository.Validators;

public class RepositoryValidator : IRepositoryValidator
{
    public IList<IInvalidField> InvalidCommonFieldsForInsert(ICommonModel model)
    {
        var fields = new List<IInvalidField>();

        if (model.Id != 0)
            fields.Add(new InvalidField("Id", "must equal zero"));

        if (model.CreatedOn != DateTime.Today)
            fields.Add(new InvalidField("CreatedOn", "must be the current date"));

        if (string.IsNullOrWhiteSpace(model.CreatedBy))
            fields.Add(new InvalidField("CreatedBy", $"must contain a value with at least one character"));
        else
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToCommonField(), "CreatedBy");
            if (!isValid)
                fields.Add(new InvalidField("CreatedBy", $"exceeds Max Length allowed {maxLength}"));
        }

        if (model.UpdatedOn.HasValue)
            fields.Add(new InvalidField("UpdatedOn", $"must not contain a date"));

        if (!string.IsNullOrWhiteSpace(model.UpdatedBy))
            fields.Add(new InvalidField("UpdatedBy", $"must not contain a value"));

        return fields;
    }

    public IList<IInvalidField> InvalidCommonFieldsForUpdate(ICommonModel model)
    {
        var fields = new List<IInvalidField>();

        if (model.Id <= 0)
            fields.Add(new InvalidField("Id", "must be greater than zero"));

        if (model.CreatedOn > DateTime.Today)
            fields.Add(new InvalidField("CreatedOn", "must be on or before the current date"));

        if (string.IsNullOrWhiteSpace(model.CreatedBy))
            fields.Add(new InvalidField("CreatedBy", $"must contain a value with at least one character"));
        else
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToCommonField(), "CreatedBy");
            if (!isValid)
                fields.Add(new InvalidField("CreatedBy", $"exceeds Max Length allowed {maxLength}"));
        }

        if (model.UpdatedOn != DateTime.Today)
            fields.Add(new InvalidField("UpdatedOn", $"must be the current date"));

        if (string.IsNullOrWhiteSpace(model.UpdatedBy))
            fields.Add(new InvalidField("UpdatedBy", $"must contain a value with at least one character"));
        else
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToCommonField(), "UpdatedBy");
            if (!isValid)
                fields.Add(new InvalidField("UpdatedBy", $"exceeds Max Length allowed {maxLength}"));
        }

        return fields;
    }
}