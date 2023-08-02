using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Abstractions.Services
{
    public interface ISuiteService : IBaseService<ISuiteView>
    {
        IEnumerable<ITestCaseView> GetTestCasesBySuiteId(int suiteId);
    }
}
