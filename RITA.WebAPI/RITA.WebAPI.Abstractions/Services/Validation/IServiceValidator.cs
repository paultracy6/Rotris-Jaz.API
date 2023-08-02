using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Validation;

namespace RITA.WebAPI.Abstractions.Services.Validation;

public interface IServiceValidator
{
    IList<IInvalidField> ValidateInsert(ICommonModel model);
    IList<IInvalidField> ValidateUpdate(ICommonModel model);
}