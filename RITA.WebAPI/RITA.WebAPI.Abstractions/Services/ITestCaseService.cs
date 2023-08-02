using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Abstractions.Services
{
    public interface ITestCaseService : IBaseService<ITestCaseView>
    {
        IEnumerable<ITestDataView> GetTestData(int testCaseId);
    }
}
