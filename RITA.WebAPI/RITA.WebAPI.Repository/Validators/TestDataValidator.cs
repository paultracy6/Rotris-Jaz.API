using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Converters;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Utility;

namespace RITA.WebAPI.Repository.Validators;

public class TestDataValidator : ITestDataModelValidator
{
    private readonly IRepositoryValidator _validator;

    public TestDataValidator(IRepositoryValidator validator)
    {
        _validator = validator;
    }

    public void ValidateInsert(ITestDataModel model)
    {
        var fields = new List<IInvalidField>();

        fields.AddRange(_validator.InvalidCommonFieldsForInsert(model));
        ValidateCommonTestDataFields(model, fields);

        if (fields.Count > 0)
            throw new ValidationException("TestData Insert Error", fields);
    }

    public void ValidateUpdate(ITestDataModel model)
    {
        var fields = new List<IInvalidField>();

        fields.AddRange(_validator.InvalidCommonFieldsForUpdate(model));
        ValidateCommonTestDataFields(model, fields);

        if (fields.Count > 0)
            throw new ValidationException("TestData Update Error", fields);
    }

    internal void ValidateCommonTestDataFields(ITestDataModel model, List<IInvalidField> fields)
    {
        if (string.IsNullOrWhiteSpace(model.Name))
            fields.Add(new InvalidField("Name", $"must contain a value with at least one character"));
        else
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToTestData(), "Name");
            if (!isValid)
                fields.Add(new InvalidField("Name", $"exceeds Max Length allowed {maxLength}"));
        }

        if (model.TestCaseId < 1)
            fields.Add(new InvalidField("TestCaseId", "must be greater than zero"));

        if (model.TestTypeId < 1)
            fields.Add(new InvalidField("TestTypeId", "must be greater than zero"));

        if (!string.IsNullOrWhiteSpace(model.Comment))
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToTestData(), "Comment");
            if (!isValid)
                fields.Add(new InvalidField("Comment", $"exceeds Max Length allowed {maxLength}"));
        }

        if (!string.IsNullOrWhiteSpace(model.SuspendedBy))
        {
            var (isValid, maxLength) = ValidationUtility.CheckMaxLengthValid(model.ToTestData(), "SuspendedBy");
            if (!isValid)
                fields.Add(new InvalidField("SuspendedBy", $"exceeds Max Length allowed {maxLength}"));
        }

        if (model.SuspendedOn != null && model.SuspendedOn > DateTime.Today)
            fields.Add(new InvalidField("SuspendedOn", $"must be on or before the current date"));
    }
}