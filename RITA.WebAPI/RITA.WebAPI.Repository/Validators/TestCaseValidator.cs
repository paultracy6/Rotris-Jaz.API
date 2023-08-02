using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Converters;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Utility;

namespace RITA.WebAPI.Repository.Validators;

public class TestCaseValidator : ITestCaseModelValidator
{
    private readonly IRepositoryValidator _validator;

    public TestCaseValidator(IRepositoryValidator validator)
    {
        _validator = validator;
    }

    public void ValidateInsert(ITestCaseModel model)
    {
        var fields = new List<IInvalidField>();

        fields.AddRange(_validator.InvalidCommonFieldsForInsert(model));
        ValidateRequiredFields(model, fields);

        if (fields.Count > 0)
            throw new ValidationException("TestCase Insertion Error", fields);
    }

    public void ValidateUpdate(ITestCaseModel model)
    {
        var fields = new List<IInvalidField>();

        fields.AddRange(_validator.InvalidCommonFieldsForUpdate(model));
        ValidateRequiredFields(model, fields);

        if (fields.Count > 0)
            throw new ValidationException("TestCase Update Error", fields);
    }

    internal static void ValidateRequiredFields(ITestCaseModel model, List<IInvalidField> fields)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
            fields.Add(new InvalidField("Name", $"must contain a value with at least one character"));
        else
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToTestCase(), "Name");
            if (!isValid)
                fields.Add(new InvalidField("Name", $"exceeds Max Length allowed {maxLength}"));
        }

        if (string.IsNullOrWhiteSpace(model.Url))
            fields.Add(new InvalidField("Url", $"must contain a value with at least one character"));
        else
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToTestCase(), "Url");
            if (!isValid)
                fields.Add(new InvalidField("Url", $"exceeds Max Length allowed {maxLength}"));
        }

        if (model.SuiteId < 1)
            fields.Add(new InvalidField("SuiteId", "must be greater than zero"));
    }
}