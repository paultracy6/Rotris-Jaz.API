using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Services;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.UnitTests.Models;
using RITA.WebAPI.UnitTests.Utilities;
using RITA.WebAPI.UnitTests.Views;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;

namespace RITA.WebAPI.UnitTests.Business;

[ExcludeFromCodeCoverage]
public class ApplicationServiceUnitTests
{
    private Mock<IApplicationRepository> _applicationRepositoryMock;
    private Mock<ILogger> _loggerMock;
    private IApplicationService _applicationService;
    private string _message = String.Empty;

    public ApplicationServiceUnitTests()
    {
        _applicationRepositoryMock = new Mock<IApplicationRepository>();
        _loggerMock = new Mock<ILogger>();
        _applicationService = new ApplicationService(_applicationRepositoryMock.Object, _loggerMock.Object);
    }

    [TestCase]
    public void GetSuiteByAppId_Success()
    {
        var models = new List<ISuiteModel>() { new SuiteModel() };
        var expected = new List<ISuiteView>() { new SuiteView() };
        _applicationRepositoryMock.Setup(x => x.GetSuitesByAppId(It.IsAny<int>()))
            .Returns(models);

        ComparerUtility.AreEqual(expected, _applicationService.GetSuiteByAppId(12));

    }

    [TestCase]
    public void GetSuiteByAppId_RepositoryException()
    {
        _message = "GetSuiteByAppId Repo Exception";
        _applicationRepositoryMock.Setup(x => x.GetSuitesByAppId(It.IsAny<int>()))
            .Throws(new RepositoryException(_message));

        Assert.Throws<RepositoryException>(() => _applicationService.GetSuiteByAppId(13));
    }

    [TestCase]
    public void GetSuiteByAppId_Exception()
    {
        _message = "GetSuiteByAppId Exception";
        _applicationRepositoryMock.Setup(x => x.GetSuitesByAppId(It.IsAny<int>()))
            .Throws(new Exception(_message));

        Assert.Throws<BusinessException>(() => _applicationService.GetSuiteByAppId(23));

        LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void GetTestCaseByAppId_Success()
    {
        var models = new List<ITestCaseModel>() { new TestCaseModel() };
        var expected = new List<ITestCaseView>() { new TestCaseView() };
        _applicationRepositoryMock.Setup(x => x.GetTestCasesByAppId(It.IsAny<int>()))
            .Returns(models);

        ComparerUtility.AreEqual(expected, _applicationService.GetTestCaseByAppId(11));
    }

    [TestCase]
    public void GetTestCaseByAppId_RepositoryException()
    {
        _message = "GetTestCaseByAppId Repo Exception";
        _applicationRepositoryMock.Setup(x => x.GetTestCasesByAppId(It.IsAny<int>()))
            .Throws(new RepositoryException(_message));

        Assert.Throws<RepositoryException>(() => _applicationService.GetTestCaseByAppId(2));
    }

    [TestCase]
    public void GetTestCaseByAppId_Exception()
    {
        _message = "GetTestCaseByAppId Exception";
        _applicationRepositoryMock.Setup(x => x.GetTestCasesByAppId(It.IsAny<int>()))
            .Throws(new Exception(_message));

        Assert.Throws<BusinessException>(() => _applicationService.GetTestCaseByAppId(22));

        LoggerUtility.VerifyException(_message, _loggerMock);
    }


}