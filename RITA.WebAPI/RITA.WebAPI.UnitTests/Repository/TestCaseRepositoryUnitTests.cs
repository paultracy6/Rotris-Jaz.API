using Moq;
using NUnit.Framework;
using RITA.EF;
using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Repository;
using RITA.WebAPI.UnitTests.Models;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;
using TestCaseModel = RITA.WebAPI.Repository.Models.TestCaseModel;

namespace RITA.WebAPI.UnitTests.Repository;

[ExcludeFromCodeCoverage]
public class TestCaseRepositoryUnitTests
{
    private Mock<ILogger> _loggerMock;
    private Mock<ITestCaseModelValidator> _validatorMock;
    private Mock<RitaContext> _contextMock;
    private ITestCaseRepository _repository;

    public TestCaseRepositoryUnitTests()
    {
        BuildData();
    }

    private string _message = string.Empty;

    [SetUp]
    public void TestSetup()
    {
        _contextMock = new Mock<RitaContext>();
        _validatorMock = new Mock<ITestCaseModelValidator>();
        _loggerMock = new Mock<ILogger>();
        _repository = new TestCaseRepository(_contextMock.Object, _validatorMock.Object, _loggerMock.Object);
    }

    [TestCase]
    public void GetById_Success()
    {
        var dateTime = DateTime.UtcNow;
        var testCase = new TestCase()
        {
            Id = 1,
            CreatedOn = dateTime
        };
        var testCaseModel = new TestCaseModel()
        {
            Id = 1,
            CreatedOn = dateTime
        };
        _contextMock
            .Setup(c => c.TestCases.Find(It.IsAny<int>()))
            .Returns(testCase);

        var actual = _repository.GetById(1);

        PropertyInfo[] propertyInfo = testCaseModel.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo)
        {
            Assert.AreEqual(property.GetValue(testCaseModel), property.GetValue(actual));
        }
    }

    [TestCase]
    public void GetById_Exception()
    {
        _message = "GetById Exception";
        _contextMock
            .Setup(d => d.TestCases.Find(It.IsAny<int>()))
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.GetById(1));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Insert_Success()
    {
        var testCaseModel = new TestCaseModel();

        _contextMock
            .Setup(d => d.TestCases.Add(It.IsAny<TestCase>()))
            .Callback((TestCase t) => t.Id = 11);
        _contextMock
            .Setup(d => d.SaveChanges())
            .Verifiable();

        ITestCaseModel returnValue = _repository.Insert(testCaseModel);
        Assert.AreEqual(11, returnValue.Id);
    }

    [TestCase]
    public void Insert_Exception()
    {
        var testCaseModel = new TestCaseModel();
        _message = "Insert Exception";
        _contextMock
            .Setup(d => d.TestCases.Add(It.IsAny<TestCase>()))
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.Insert(testCaseModel));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Insert_ValidationException()
    {
        var testCaseModel = new TestCaseModel();
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("TestCase Data contains invalid fields", new[] { field });
        _validatorMock
            .Setup(v => v.ValidateInsert(It.IsAny<TestCaseModel>()))
            .Throws(validationException);

        Assert.Throws<ValidationException>(() => _repository.Insert(testCaseModel));
    }

    [TestCase]
    public void Update_Success()
    {
        var testCaseModel = new TestCaseModel() { Id = 12 };
        _contextMock.Setup(d => d.TestCases.Update(It.IsAny<TestCase>()))
            .Callback((TestCase s) => s.Id = 12);
        _contextMock.Setup(d => d.SaveChanges())
            .Verifiable();

        ITestCaseModel returnValue = _repository.Update(testCaseModel);
        Assert.AreEqual(12, returnValue.Id);
    }

    [TestCase]
    public void Update_Exception()
    {
        var testCaseModel = new TestCaseModel();
        _message = "Update Exception";
        _contextMock
            .Setup(d => d.TestCases.Update(It.IsAny<TestCase>()))
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.Update(testCaseModel));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Update_ValidationException()
    {
        var testCaseModel = new TestCaseModel();
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("TestCase Data contains invalid fields", new[] { field });
        _validatorMock
            .Setup(v => v.ValidateUpdate(It.IsAny<TestCaseModel>()))
            .Throws(validationException);

        Assert.Throws<ValidationException>(() => _repository.Update(testCaseModel));
    }

    [TestCase]
    public void Delete_Success()
    {
        var testCase = new TestCase()
        {
            Id = 3,
            CreatedOn = DateTime.Now
        };

        _contextMock
            .Setup(c => c.TestCases.Find(It.IsAny<int>()))
            .Returns(testCase);
        _contextMock.Setup(d => d.TestCases.Remove(It.IsAny<TestCase>()))
            .Callback((TestCase s) => s.Id = 3);
        _contextMock
            .Setup(d => d.SaveChanges())
            .Verifiable();

        var returnValue = _repository.Delete(3);
        Assert.AreEqual(3, returnValue);
    }

    [TestCase]
    public void Delete_SuiteNotFound()
    {
        _contextMock
            .Setup(c => c.TestCases.Find(It.IsAny<int>()))
            .Returns((TestCase)null);

        var returnValue = _repository.Delete(4);
        Assert.AreEqual(4, returnValue);
    }

    [TestCase]
    public void Delete_Exception()
    {
        _message = "Delete Exception";
        var testCase = new TestCase()
        {
            Id = 13,
            CreatedOn = DateTime.Now
        };

        _contextMock
            .Setup(c => c.TestCases.Find(It.IsAny<int>()))
            .Returns(testCase);
        _contextMock.Setup(d => d.TestCases.Remove(It.IsAny<TestCase>()))
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.Delete(3));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void GetTestData_Success()
    {
        var testDataId = 102;
        TestCaseRepository testCases = new TestCaseRepository(Context, _validatorMock.Object, _loggerMock.Object);
        var actual = testCases.GetTestData(testDataId);

        Assert.AreEqual(2, actual.Count());
    }

    [TestCase]
    public void GetTestData_Exception()
    {
        _message = "GetTestData Exception";
        _contextMock
            .Setup(d => d.TestData)
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.GetTestData(1));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }
}
