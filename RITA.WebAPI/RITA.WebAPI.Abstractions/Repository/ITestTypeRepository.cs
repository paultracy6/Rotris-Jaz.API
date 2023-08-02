using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Abstractions.Repository
{
    public interface ITestTypeRepository
    {
        IEnumerable<ITestTypeModel> GetAll();
    }
}
