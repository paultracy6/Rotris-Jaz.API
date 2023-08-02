using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Abstractions.Repository;

public interface IApplicationRepository
{
    IEnumerable<ISuiteModel> GetSuitesByAppId(int appId);

    IEnumerable<ITestCaseModel> GetTestCasesByAppId(int appId);
}