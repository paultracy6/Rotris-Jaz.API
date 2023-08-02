using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Abstractions.WebAPI.Validation;
using RITA.WebAPI.Api.Models;

namespace RITA.WebAPI.Api.Validators;

public class TestCaseControllerValidator : ITestCaseControllerValidator
{
    public void ValidateInsert(ITestCaseView view)
    {
        var fields = new List<IInvalidField>();

        if (view.Id != 0)
            fields.Add(new InvalidField("Id", "must be zero"));

        if (fields.Count > 0)
            throw new ValidationException("Insertion Error", fields);
    }

    public void ValidateUpdate(int id, ITestCaseView view)
    {
        var fields = new List<IInvalidField>();

        if (id < 1)
            fields.Add(new InvalidField("Id", "must be greater than zero"));

        if (id != view.Id)
            fields.Add(new InvalidField("Id and Entity Id", "must be equal"));

        if (fields.Count > 0)
            throw new ValidationException("Update Error", fields);
    }
}