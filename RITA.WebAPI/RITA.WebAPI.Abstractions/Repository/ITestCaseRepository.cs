using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Abstractions.Repository
{
    public interface ITestCaseRepository : IBaseRepository<ITestCaseModel>
    {
        IEnumerable<ITestDataModel> GetTestData(int testCaseId);
    }
}
