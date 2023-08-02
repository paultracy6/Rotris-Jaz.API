using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Abstractions.Repository
{
    public interface ISuiteRepository : IBaseRepository<ISuiteModel>
    {
        IEnumerable<ITestCaseModel> GetTestCases(int suiteId);
    }
}
