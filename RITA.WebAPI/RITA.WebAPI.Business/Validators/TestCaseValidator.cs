using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Business.Models;

namespace RITA.WebAPI.Business.Validators;

public class TestCaseValidator : ITestCaseValidator
{
    private readonly IServiceValidator _serviceValidator;

    public TestCaseValidator(IServiceValidator serviceValidator)
    {
        _serviceValidator = serviceValidator;
    }

    public void ValidateInsert(ITestCaseModel model)
    {
        var fields = new List<IInvalidField>();
        fields.AddRange(_serviceValidator.ValidateInsert(model));

        if (model.SuiteId < 1)
            fields.Add(new InvalidField("SuiteId", "must be greater than zero"));

        if (string.IsNullOrWhiteSpace(model.Url))
            fields.Add(new InvalidField("Url", "cannot be empty"));

        if (string.IsNullOrWhiteSpace(model.Name))
            fields.Add(new InvalidField("Name", "cannot be empty"));

        if (fields.Count > 0)
            throw new ValidationException("Test Case Insertion Error", fields);
    }

    public void ValidateUpdate(ITestCaseModel model)
    {
        var fields = new List<IInvalidField>();
        fields.AddRange(_serviceValidator.ValidateUpdate(model));

        if (model.SuiteId < 1)
            fields.Add(new InvalidField("SuiteId", "must be greater than zero"));

        if (string.IsNullOrWhiteSpace(model.Url))
            fields.Add(new InvalidField("Url", "cannot be empty"));

        if (string.IsNullOrWhiteSpace(model.Name))
            fields.Add(new InvalidField("Name", "cannot be empty"));

        if (fields.Count > 0)
            throw new ValidationException("Test Case Update Error", fields);
    }
}