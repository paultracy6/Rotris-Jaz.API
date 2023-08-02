using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Business.Models;

namespace RITA.WebAPI.Business.Validators;

public class SuiteValidator : ISuiteValidator
{
    private readonly IServiceValidator _serviceValidator;


    public SuiteValidator(IServiceValidator serviceValidator)
    {
        _serviceValidator = serviceValidator;
    }

    public void ValidateInsert(ISuiteModel model)
    {
        var fields = new List<IInvalidField>();
        fields.AddRange(_serviceValidator.ValidateInsert(model));

        if (model.AppId < 1)
            fields.Add(new InvalidField("AppId", "must be greater than zero"));

        if (string.IsNullOrEmpty(model.Name))
            fields.Add(new InvalidField("Name", "cannot be empty"));

        if (fields.Count > 0)
            throw new ValidationException("Suite Insertion Error", fields);
    }

    public void ValidateUpdate(ISuiteModel model)
    {
        var fields = new List<IInvalidField>();
        fields.AddRange(_serviceValidator.ValidateUpdate(model));

        if (model.AppId < 1)
            fields.Add(new InvalidField("AppId", "must be greater than zero"));

        if (string.IsNullOrEmpty(model.Name))
            fields.Add(new InvalidField("Name", "cannot be empty"));

        if (fields.Count > 0)
            throw new ValidationException("Suite Update Error", fields);
    }
}