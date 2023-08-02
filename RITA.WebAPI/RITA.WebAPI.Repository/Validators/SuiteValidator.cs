using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Converters;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Utility;

namespace RITA.WebAPI.Repository.Validators;

public class SuiteValidator : ISuiteModelValidator
{
    private readonly IRepositoryValidator _validator;

    public SuiteValidator(IRepositoryValidator validator)
    {
        _validator = validator;
    }

    public void ValidateInsert(ISuiteModel model)
    {
        var fields = new List<IInvalidField>();

        fields.AddRange(_validator.InvalidCommonFieldsForInsert(model));
        ValidateRequiredFields(model, fields);

        if (fields.Count > 0)
            throw new ValidationException("Suite Insertion Error", fields);
    }

    public void ValidateUpdate(ISuiteModel model)
    {
        var fields = new List<IInvalidField>();

        fields.AddRange(_validator.InvalidCommonFieldsForUpdate(model));
        ValidateRequiredFields(model, fields);

        if (fields.Count > 0)
            throw new ValidationException("Suite Update Error", fields);
    }

    internal static void ValidateRequiredFields(ISuiteModel model, List<IInvalidField> fields)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
            fields.Add(new InvalidField("Name", $"must contain a value with at least one character"));
        else
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToSuite(), "Name");
            if (!isValid)
                fields.Add(new InvalidField("Name", $"exceeds Max Length allowed {maxLength}"));
        }

        if (model.AppId < 1)
            fields.Add(new InvalidField("AppId", "must be greater than zero"));
    }
}