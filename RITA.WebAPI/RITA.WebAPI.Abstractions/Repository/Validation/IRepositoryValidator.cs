using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Validation;

namespace RITA.WebAPI.Abstractions.Repository.Validation;

public interface IRepositoryValidator
{
    IList<IInvalidField> InvalidCommonFieldsForInsert(ICommonModel model);

    IList<IInvalidField> InvalidCommonFieldsForUpdate(ICommonModel model);
}