using Moq;
using NUnit.Framework;
using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Repository;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;

namespace RITA.WebAPI.UnitTests.Repository;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
public class ApplicationRepositoryUnitTests
{
    private Mock<ILogger> _loggerMock;
    private Mock<RitaContext> _contextMock;
    private IApplicationRepository _repository;

    private string _message = string.Empty;

    public ApplicationRepositoryUnitTests()
    {
        BuildData();
    }

    [SetUp]
    public void TestSetup()
    {
        _contextMock = new Mock<RitaContext>();
        _loggerMock = new Mock<ILogger>();
        _repository = new ApplicationRepository(_contextMock.Object, _loggerMock.Object);
    }

    [TestCase]
    public void GetSuitesByAppId_Success()
    {
        var application = new ApplicationRepository(Context, _loggerMock.Object);
        IEnumerable<ISuiteModel> suiteList = application.GetSuitesByAppId(10);

        Assert.AreEqual(2, suiteList.Count());
        foreach (var suite in suiteList)
        {
            Assert.AreEqual(10, suite.AppId);
        }

    }

    [TestCase]
    public void GetSuitesByAppId_Success_1of3()
    {
        const int appId = 11;
        var application = new ApplicationRepository(Context, _loggerMock.Object);
        IEnumerable<ISuiteModel> suiteList = application.GetSuitesByAppId(appId);

        Assert.AreEqual(1, suiteList.Count());
        foreach (var suite in suiteList)
        {
            Assert.AreEqual(appId, suite.AppId, $"AppId does not equal {appId}");
        }
    }

    [TestCase]
    public void GetSuitesByAppId_Exception()
    {
        _message = "GetSuitesByAppId Exception";
        _contextMock.Setup(d => d.Suites)
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.GetSuitesByAppId(1));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void GetTestCasesByAppId_Success()
    {
        const int appId = 10;
        ApplicationRepository testCases = new ApplicationRepository(Context, _loggerMock.Object);
        IEnumerable<ITestCaseModel> testCaseList = testCases.GetTestCasesByAppId(appId);

        Assert.AreEqual(3, testCaseList.Count());
    }

    [TestCase]
    public void GetTestCasesByAppId_Success_1of4()
    {
        const int appId = 11;
        ApplicationRepository testCases = new ApplicationRepository(Context, _loggerMock.Object);
        IEnumerable<ITestCaseModel> testCaseList = testCases.GetTestCasesByAppId(appId);

        Assert.AreEqual(1, testCaseList.Count());
    }

    [TestCase]
    public void GetTestCasesByAppId_Exception()
    {
        _message = "GetTestCasesByAppId Exception";
        _contextMock.Setup(d => d.Suites)
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.GetTestCasesByAppId(1));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }
}