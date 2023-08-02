using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Abstractions.Services;

public interface IApplicationService
{
    IEnumerable<ISuiteView> GetSuiteByAppId(int appId);

    IEnumerable<ITestCaseView> GetTestCaseByAppId(int appId);
}