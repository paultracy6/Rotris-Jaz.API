using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Converters;
using RITA.WebAPI.Business.Services;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.UnitTests.Models;
using RITA.WebAPI.UnitTests.Utilities;
using RITA.WebAPI.UnitTests.Views;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;

namespace RITA.WebAPI.UnitTests.Business;

[ExcludeFromCodeCoverage]
public class TestCaseServiceUnitTests
{
    private readonly Mock<ITestCaseRepository> _testCaseRepositoryMock;
    private readonly Mock<ITestCaseValidator> _validatorMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly ITestCaseService _testCaseService;
    private readonly ITestCaseView _testCaseView = new TestCaseView();
    private readonly ITestCaseModel _testCaseModel = new TestCaseModel();
    private string _message = string.Empty;

    public TestCaseServiceUnitTests()
    {
        _testCaseRepositoryMock = new Mock<ITestCaseRepository>();
        _validatorMock = new Mock<ITestCaseValidator>();
        _loggerMock = new Mock<ILogger>();
        _testCaseService = new TestCaseService(_testCaseRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object);
    }

    [SetUp]
    public void TestSetup()
    {
        _testCaseRepositoryMock.Reset();
        _validatorMock.Reset();
        _loggerMock.Reset();
    }

    [TestCase]
    public void Delete_Success()
    {
        _testCaseRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
            .Returns(10);
        var results = _testCaseService.Delete(10);

        Assert.AreEqual(10, results);
    }

    [TestCase]
    public void Delete_RepositoryException()
    {
        _message = "Test Delete Repo Exception";
        _testCaseRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
            .Throws(new RepositoryException(_message));

        Assert.Throws<RepositoryException>(() => _testCaseService.Delete(10));
    }

    [TestCase]
    public void Delete_Exception()
    {
        _message = "Test Delete Exception";
        _testCaseRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
            .Throws(new Exception(_message));

        Assert.Throws<BusinessException>(() => _testCaseService.Delete(10));

        LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Insert_Success()
    {
        _testCaseModel.Id = 3;
        _testCaseView.Id = 0;
        _testCaseRepositoryMock.Setup(x => x.Insert(It.IsAny<ITestCaseModel>()))
            .Returns(_testCaseModel);
        var results = _testCaseService.Insert(_testCaseView, "heyLoser");

        ComparerUtility.AreEqual(_testCaseModel.Id, results.Id);
    }

    [TestCase]
    public void Insert_RepositoryException()
    {
        _message = "Test Insert Repo Exception";
        _testCaseRepositoryMock.Setup(x => x.Insert(It.IsAny<ITestCaseModel>()))
            .Throws(new RepositoryException(_message));

        Assert.Throws<RepositoryException>(() => _testCaseService.Insert(_testCaseView, "API"));
    }

    [TestCase]
    public void Insert_Exception()
    {
        _message = "Test Insert Exception";
        _validatorMock.Setup(x => x.ValidateInsert(It.IsAny<ITestCaseModel>()))
            .Throws(new Exception(_message));

        Assert.Throws<BusinessException>(() => _testCaseService.Insert(_testCaseView, "API"));

        LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Insert_ValidationException()
    {
        var validationException = new ValidationException("BADDATA", Array.Empty<IInvalidField>());
        _validatorMock.Setup(x => x.ValidateInsert(It.IsAny<ITestCaseModel>()))
            .Throws(validationException);

        Assert.Throws<ValidationException>(() => _testCaseService.Insert(_testCaseView, "API"));
    }

    [TestCase]
    public void Update_Success()
    {
        _testCaseModel.Name = "SoSoDef";
        _testCaseView.Name = "YabbaDabbaDoo";
        _testCaseRepositoryMock.Setup(x => x.Update(It.IsAny<ITestCaseModel>()))
            .Returns(_testCaseModel);
        var results = _testCaseService.Update(_testCaseView, "YeaYeaYea");

        Assert.AreEqual("SoSoDef", results.Name);
    }

    [TestCase]
    public void Update_RepositoryException()
    {
        _message = "Test Update Repo Exception";
        _testCaseRepositoryMock.Setup(x => x.Update(It.IsAny<ITestCaseModel>()))
            .Throws(new RepositoryException(_message));

        Assert.Throws<RepositoryException>(() => _testCaseService.Update(_testCaseView, "API"));
    }

    [TestCase]
    public void Update_Exception()
    {
        _message = "Test Update Exception";
        _validatorMock.Setup(x => x.ValidateUpdate(It.IsAny<ITestCaseModel>()))
            .Throws(new Exception(_message));

        Assert.Throws<BusinessException>(() => _testCaseService.Update(_testCaseView, "API"));

        LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Update_ValidationException()
    {
        var validationException = new ValidationException("BADDATA", Array.Empty<IInvalidField>());
        _validatorMock.Setup(x => x.ValidateUpdate(It.IsAny<ITestCaseModel>()))
            .Throws(validationException);

        Assert.Throws<ValidationException>(() => _testCaseService.Update(_testCaseView, "API"));
    }

    [TestCase]
    public void GetById_Success()
    {
        _testCaseRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(_testCaseModel);

        ComparerUtility.AreEqual(_testCaseModel.ToView(), _testCaseService.GetById(25));
    }

    [TestCase]
    public void GetById_NotFound()
    {
        _testCaseRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<ITestCaseModel>(null);

        Assert.Null(_testCaseService.GetById(25));
    }

    [TestCase]
    public void GetTestData_Success()
    {
        IEnumerable<ITestDataView> expected = new List<ITestDataView>() { new TestDataView() };
        _testCaseRepositoryMock.Setup(x => x.GetTestData(It.IsAny<int>()))
            .Returns(expected.ToModels());

        ComparerUtility.AreEqual(expected, _testCaseService.GetTestData(22));
    }

    [TestCase]
    public void GetTestData_RepositoryException()
    {
        _message = "Test GetTestData Repo Exception";
        _testCaseRepositoryMock.Setup(x => x.GetTestData(It.IsAny<int>()))
            .Throws(new RepositoryException(_message));

        Assert.Throws<RepositoryException>(() => _testCaseService.GetTestData(21));
    }

    [TestCase]
    public void GetTestData_Exception()
    {
        _message = "Test GetTestData Exception";
        _testCaseRepositoryMock.Setup(x => x.GetTestData(It.IsAny<int>()))
            .Throws(new Exception(_message));

        Assert.Throws<BusinessException>(() => _testCaseService.GetTestData(20));

        LoggerUtility.VerifyException(_message, _loggerMock);
    }
}