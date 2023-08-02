using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Abstractions.Services
{
    public interface ITestTypeService
    {
        IEnumerable<ITestTypeView> GetAll();
    }
}
