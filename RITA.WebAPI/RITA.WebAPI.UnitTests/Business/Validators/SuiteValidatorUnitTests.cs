using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Business.Validators;
using RITA.WebAPI.UnitTests.Common;
using RITA.WebAPI.UnitTests.Models;

namespace RITA.WebAPI.UnitTests.Business.Validators;

[ExcludeFromCodeCoverage]
public class SuiteValidatorUnitTests
{
    private ISuiteModel _insertSuiteModel;
    private ISuiteModel _updateSuiteModel;
    private readonly ISuiteValidator _suiteValidator;
    private const string InsertMessageTitle = "Suite Insertion Error";
    private const string UpdateMessageTitle = "Suite Update Error";

    public SuiteValidatorUnitTests()
    {
        var validator = new Mock<IServiceValidator>();
        _suiteValidator = new SuiteValidator(validator.Object);
        validator.Setup(x => x.ValidateInsert(It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
        validator.Setup(x => x.ValidateUpdate(It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
    }

    [SetUp]
    public void TestSetup()
    {
        _insertSuiteModel = new SuiteModel()
        {
            AppId = 1,
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 0,
            Name = "Jazzy"

        };
        _updateSuiteModel = new SuiteModel()
        {
            AppId = 1,
            UpdatedBy = "JazzUpdate",
            UpdatedOn = DateTime.Today,
            Id = 7,
            Name = "JazzyJ"
        };
    }

    [TestCase]
    public void ValidateInsert_Success()
    {

        Assert.DoesNotThrow(() => _suiteValidator.ValidateInsert(_insertSuiteModel));

    }

    [TestCase]
    public void ValidateInsert_AppId_EqualToZero_Failure()
    {
        var field = new InvalidField("AppId", "must be greater than zero");
        _insertSuiteModel.AppId = 0;

        CommonValidation.InsertTest(field, _suiteValidator, _insertSuiteModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_Name_Failure()
    {
        var field = new InvalidField("Name", "cannot be empty");
        _insertSuiteModel.Name = string.Empty;

        CommonValidation.InsertTest(field, _suiteValidator, _insertSuiteModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_Success()
    {
        Assert.DoesNotThrow(() => _suiteValidator.ValidateUpdate(_updateSuiteModel));
    }

    [TestCase]
    public void ValidateUpdate_AppId_EqualToZero_Failure()
    {
        var field = new InvalidField("AppId", "must be greater than zero");
        _updateSuiteModel.AppId = 0;

        CommonValidation.UpdateTest(field, _suiteValidator, _updateSuiteModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_Name_Failure()
    {
        var field = new InvalidField("Name", "cannot be empty");
        _updateSuiteModel.Name = string.Empty;

        CommonValidation.UpdateTest(field, _suiteValidator, _updateSuiteModel, UpdateMessageTitle);
    }
}