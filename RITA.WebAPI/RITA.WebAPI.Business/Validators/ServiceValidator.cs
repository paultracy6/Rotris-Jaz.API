using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Business.Models;

namespace RITA.WebAPI.Business.Validators;

public class ServiceValidator : IServiceValidator
{
    public IList<IInvalidField> ValidateInsert(ICommonModel model)
    {
        var fields = new List<IInvalidField>();

        if (model.Id != 0)
            fields.Add(new InvalidField("Id", "must be zero"));

        if (model.CreatedOn != DateTime.Today)
            fields.Add(new InvalidField("CreatedOn", "must be current date"));

        if (string.IsNullOrWhiteSpace(model.CreatedBy))
            fields.Add(new InvalidField("CreatedBy", "cannot be empty on insert"));

        if (model.UpdatedOn.HasValue)
            fields.Add(new InvalidField("UpdatedOn", "must be empty"));

        if (!string.IsNullOrEmpty(model.UpdatedBy))
            fields.Add(new InvalidField("UpdatedBy", "must be empty"));

        return fields;
    }

    public IList<IInvalidField> ValidateUpdate(ICommonModel model)
    {
        var fields = new List<IInvalidField>();
        if (model.Id < 1)
            fields.Add(new InvalidField("Id", "must be greater than zero"));

        if (model.UpdatedOn != DateTime.Today)
            fields.Add(new InvalidField("UpdatedOn", "must be current date"));

        if (string.IsNullOrWhiteSpace(model.UpdatedBy))
            fields.Add(new InvalidField("UpdatedBy", "cannot be empty on update"));

        return fields;
    }
}