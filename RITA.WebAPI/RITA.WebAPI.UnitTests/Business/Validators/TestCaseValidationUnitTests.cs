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
public class TestCaseValidationUnitTests
{
    private ITestCaseModel _inserTestCaseModel;
    private ITestCaseModel _updaTestCaseModel;
    private readonly ITestCaseValidator _testCaseValidator;
    private const string InsertMessageTitle = "Test Case Insertion Error";
    private const string UpdateMessageTitle = "Test Case Update Error";

    public TestCaseValidationUnitTests()
    {
        var validator = new Mock<IServiceValidator>();
        _testCaseValidator = new TestCaseValidator(validator.Object);
        validator.Setup(x => x.ValidateInsert(It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
        validator.Setup(x => x.ValidateUpdate(It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
    }

    [SetUp]
    public void TestSetup()
    {
        _inserTestCaseModel = new TestCaseModel()
        {
            Id = 0,
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            SuiteId = 3,
            Url = "myfancypantswebsite.com",
            Name = "Jazzy"
        };
        _updaTestCaseModel = new TestCaseModel()
        {
            Id = 3,
            UpdatedBy = "Jazz",
            UpdatedOn = DateTime.Today,
            SuiteId = 3,
            Url = "myfancypantswebsite.com",
            Name = "Jazzy"
        };
    }

    [TestCase]
    public void ValidateInsert_Success()
    {
        Assert.DoesNotThrow(() => _testCaseValidator.ValidateInsert(_inserTestCaseModel));
    }

    [TestCase]
    public void ValidateInsert_SuiteId_NotGreaterThanZero_Failure()
    {
        var field = new InvalidField("SuiteId", "must be greater than zero");
        _inserTestCaseModel.SuiteId = 0;

        CommonValidation.InsertTest(field, _testCaseValidator, _inserTestCaseModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_Url_Failure()
    {
        var field = new InvalidField("Url", "cannot be empty");
        _inserTestCaseModel.Url = string.Empty;

        CommonValidation.InsertTest(field, _testCaseValidator, _inserTestCaseModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_Name_Failure()
    {
        var field = new InvalidField("Name", "cannot be empty");
        _inserTestCaseModel.Name = string.Empty;

        CommonValidation.InsertTest(field, _testCaseValidator, _inserTestCaseModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_Success()
    {
        Assert.DoesNotThrow(() => _testCaseValidator.ValidateUpdate(_updaTestCaseModel));
    }

    [TestCase]
    public void ValidateUpdate_SuiteId_NotGreaterThanZero_Failure()
    {
        var field = new InvalidField("SuiteId", "must be greater than zero");
        _updaTestCaseModel.SuiteId = 0;

        CommonValidation.UpdateTest(field, _testCaseValidator, _updaTestCaseModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_Url_Failure()
    {
        var field = new InvalidField("Url", "cannot be empty");
        _updaTestCaseModel.Url = string.Empty;

        CommonValidation.UpdateTest(field, _testCaseValidator, _updaTestCaseModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_Name_Failure()
    {
        var field = new InvalidField("Name", "cannot be empty");
        _updaTestCaseModel.Name = string.Empty;

        CommonValidation.UpdateTest(field, _testCaseValidator, _updaTestCaseModel, UpdateMessageTitle);
    }
}