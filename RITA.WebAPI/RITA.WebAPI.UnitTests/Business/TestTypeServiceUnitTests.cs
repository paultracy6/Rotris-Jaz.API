using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Api.Models;
using RITA.WebAPI.Business.Services;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.UnitTests.Models;
using RITA.WebAPI.UnitTests.Utilities;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;

namespace RITA.WebAPI.UnitTests.Business;

[ExcludeFromCodeCoverage]
public class TestTypeServiceUnitTests
{
    private Mock<ITestTypeRepository> _testTypeRepositoryMock;
    private Mock<ILogger> _loggerMock;
    private ITestTypeService _testTypeService;
    private string _message = String.Empty;

    public TestTypeServiceUnitTests()
    {

    }

    [SetUp]
    public void TestSetup()
    {
        _testTypeRepositoryMock = new Mock<ITestTypeRepository>();
        _loggerMock = new Mock<ILogger>();
        _testTypeService = new TestTypeService(_testTypeRepositoryMock.Object, _loggerMock.Object);

    }

    [TestCase]
    public void GetAll_Success()
    {
        var expected = new List<ITestTypeModel>() { new TestTypeModel() };
        _testTypeRepositoryMock.Setup(x => x.GetAll())
            .Returns(expected);

        Assert.AreEqual(expected, _testTypeService.GetAll());
    }

    [TestCase]
    public void GetAll_RepositoryException()
    {
        _message = "Test GetAll Repo Exception";
        _testTypeRepositoryMock.Setup(x => x.GetAll())
            .Throws(new RepositoryException(_message));

        Assert.Throws<RepositoryException>(() => _testTypeService.GetAll());
    }

    [TestCase]
    public void GetAll_Exception()
    {
        _message = "Test GetAll Exception";
        _testTypeRepositoryMock.Setup(x => x.GetAll())
            .Throws(new Exception(_message));

        Assert.Throws<BusinessException>(() => _testTypeService.GetAll());

        LoggerUtility.VerifyException(_message, _loggerMock);
    }
}