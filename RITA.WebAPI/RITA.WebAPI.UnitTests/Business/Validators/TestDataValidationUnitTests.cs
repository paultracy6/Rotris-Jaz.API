using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Business.Validators;
using RITA.WebAPI.UnitTests.Common;
using RITA.WebAPI.UnitTests.Models;
using System.Diagnostics.CodeAnalysis;

namespace RITA.WebAPI.UnitTests.Business.Validators;

[ExcludeFromCodeCoverage]
public class TestDataValidationUnitTests
{
    private ITestDataModel _insertTestDataModel;
    private ITestDataModel _updateTestDataModel;
    private readonly ITestDataValidator _testDataValidator;
    private const string InsertMessageTitle = "Test Data Insertion Error";
    private const string UpdateMessageTitle = "Test Data Update Error";

    public TestDataValidationUnitTests()
    {
        var validator = new Mock<IServiceValidator>();
        _testDataValidator = new TestDataValidator(validator.Object);
        validator.Setup(x => x.ValidateInsert(It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
        validator.Setup(x => x.ValidateUpdate(It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
    }

    [SetUp]
    public void TestSetup()
    {
        _insertTestDataModel = new TestDataModel()
        {
            Id = 0,
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            TestTypeId = 1,
            TestCaseId = 1,
            Name = "Jazzy",
            IsSuspended = false
        };
        _updateTestDataModel = new TestDataModel()
        {
            Id = 3,
            UpdatedBy = "Jazz",
            UpdatedOn = DateTime.Today,
            TestTypeId = 1,
            TestCaseId = 1,
            Name = "Jazzy",
            IsSuspended = false
        };
    }

    [TestCase]
    public void ValidateInsert_Success()
    {
        Assert.DoesNotThrow(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
    }

    [TestCase]
    public void ValidateInsert_TestTypeId_NotGreaterThanZero_Failure()
    {
        var field = new InvalidField("TestTypeId", "must be greater than zero");
        _insertTestDataModel.TestTypeId = 0;

        CommonValidation.InsertTest(field, _testDataValidator, _insertTestDataModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_TestCaseId_NotGreaterThanZero_Failure()
    {
        var field = new InvalidField("TestCaseId", "must be greater than zero");
        _insertTestDataModel.TestCaseId = 0;

        CommonValidation.InsertTest(field, _testDataValidator, _insertTestDataModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_Name_Failure()
    {
        var field = new InvalidField("Name", "cannot be empty");
        _insertTestDataModel.Name = string.Empty;

        CommonValidation.InsertTest(field, _testDataValidator, _insertTestDataModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_SuspendedOn_Failure()
    {
        var field = new InvalidField("SuspendedOn", "cannot be blank when IsSuspended is true");
        _insertTestDataModel.IsSuspended = true;
        _insertTestDataModel.SuspendedBy = "Jazz";

        CommonValidation.InsertTest(field, _testDataValidator, _insertTestDataModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_SuspendedBy_Failure()
    {
        var field = new InvalidField("SuspendedBy", "cannot be blank when IsSuspended is true");
        _insertTestDataModel.IsSuspended = true;
        _insertTestDataModel.SuspendedOn = DateTime.Today;

        CommonValidation.InsertTest(field, _testDataValidator, _insertTestDataModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_Success()
    {
        Assert.DoesNotThrow(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
    }

    [TestCase]
    public void ValidateUpdate_TestTypeId_NotGreaterThanZero_Failure()
    {
        var field = new InvalidField("TestTypeId", "must be greater than zero");
        _updateTestDataModel.TestTypeId = 0;

        CommonValidation.UpdateTest(field, _testDataValidator, _updateTestDataModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_TestCaseId_NotGreaterThanZero_Failure()
    {
        var field = new InvalidField("TestCaseId", "must be greater than zero");
        _updateTestDataModel.TestCaseId = 0;

        CommonValidation.UpdateTest(field, _testDataValidator, _updateTestDataModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_Name_Failure()
    {
        var field = new InvalidField("Name", "cannot be empty");
        _updateTestDataModel.Name = string.Empty;

        CommonValidation.UpdateTest(field, _testDataValidator, _updateTestDataModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_SuspendedOn_Failure()
    {
        var field = new InvalidField("SuspendedOn", "cannot be blank when IsSuspended is true");
        _updateTestDataModel.IsSuspended = true;
        _updateTestDataModel.SuspendedBy = "Jazz";

        CommonValidation.UpdateTest(field, _testDataValidator, _updateTestDataModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_SuspendedBy_Failure()
    {
        var field = new InvalidField("SuspendedBy", "cannot be blank when IsSuspended is true");
        _updateTestDataModel.IsSuspended = true;
        _updateTestDataModel.SuspendedOn = DateTime.Today;

        CommonValidation.UpdateTest(field, _testDataValidator, _updateTestDataModel, UpdateMessageTitle);
    }
}