using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Business.Models;


namespace RITA.WebAPI.Business.Validators;

public class TestDataValidator : ITestDataValidator
{
    private readonly IServiceValidator _serviceValidator;

    public TestDataValidator(IServiceValidator serviceValidator)
    {
        _serviceValidator = serviceValidator;
    }

    public void ValidateInsert(ITestDataModel model)
    {
        var fields = new List<IInvalidField>();
        fields.AddRange(_serviceValidator.ValidateInsert(model));

        if (model.TestTypeId < 1)
            fields.Add(new InvalidField("TestTypeId", "must be greater than zero"));

        if (model.TestCaseId < 1)
            fields.Add(new InvalidField("TestCaseId", "must be greater than zero"));

        if (string.IsNullOrWhiteSpace(model.Name))
            fields.Add(new InvalidField("Name", "cannot be empty"));

        if (model.IsSuspended)
        {
            if (string.IsNullOrWhiteSpace(model.SuspendedBy))
                fields.Add(new InvalidField("SuspendedBy", "cannot be blank when IsSuspended is true"));
            if (model.SuspendedOn > DateTime.Today || model.SuspendedOn == null)
                fields.Add(new InvalidField("SuspendedOn", "cannot be blank when IsSuspended is true"));
        }


        if (fields.Count > 0)
            throw new ValidationException("Test Data Insertion Error", fields);
    }

    public void ValidateUpdate(ITestDataModel model)
    {
        var fields = new List<IInvalidField>();
        fields.AddRange(_serviceValidator.ValidateUpdate(model));

        if (model.TestTypeId < 1)
            fields.Add(new InvalidField("TestTypeId", "must be greater than zero"));

        if (model.TestCaseId < 1)
            fields.Add(new InvalidField("TestCaseId", "must be greater than zero"));

        if (string.IsNullOrWhiteSpace(model.Name))
            fields.Add(new InvalidField("Name", "cannot be empty"));

        if (model.IsSuspended == true)
        {
            if (string.IsNullOrWhiteSpace(model.SuspendedBy))
                fields.Add(new InvalidField("SuspendedBy", "cannot be blank when IsSuspended is true"));
            if (model.SuspendedOn == null || model.SuspendedOn == DateTime.MinValue)
                fields.Add(new InvalidField("SuspendedOn", "cannot be blank when IsSuspended is true"));
        }

        if (fields.Count > 0)
            throw new ValidationException("Test Data Update Error", fields);
    }
}