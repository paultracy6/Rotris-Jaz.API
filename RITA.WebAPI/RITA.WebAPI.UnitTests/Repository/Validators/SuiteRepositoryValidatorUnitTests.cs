using Moq;
using NUnit.Framework;
using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Validators;
using RITA.WebAPI.UnitTests.Common;
using RITA.WebAPI.UnitTests.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static RITA.WebAPI.Repository.Utility.ValidationUtility;
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;

namespace RITA.WebAPI.UnitTests.Repository.Validators;

[ExcludeFromCodeCoverage]
public class SuiteRepositoryValidatorUnitTests
{
    private SuiteValidator _suiteValidator;
    private SuiteModel _insertSuiteModel;
    private SuiteModel _updateSuiteModel;
    private Mock<IRepositoryValidator> _repositoryValidator;

    private const string InsertMessageTitle = "Suite Insertion Error";
    private const string UpdateMessageTitle = "Suite Update Error";

    public SuiteRepositoryValidatorUnitTests()
    {
        BuildData();

        _repositoryValidator = new Mock<IRepositoryValidator>();
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForInsert(
            It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForUpdate(
            It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
    }

    [SetUp]
    public void TestSetUp()
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
            AppId = 2,
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 13,
            Name = "Jazzy",
            UpdatedOn = DateTime.Today,
            UpdatedBy = "Paul"
        };

        _suiteValidator = new SuiteValidator(_repositoryValidator.Object);
    }

    [TestCase]
    public void ValidateInsert_Success()
    {
        var suite = new Suite();
        var maxLength = suite.GetAttributeFrom<MaxLengthAttribute>("Name");
        _insertSuiteModel.Name = StringBuilders.BuildStringForLength(maxLength.Length);

        Assert.DoesNotThrow(() => _suiteValidator.ValidateInsert(_insertSuiteModel));
    }

    [TestCase]
    public void ValidateInsert_NameIsNull()
    {
        _insertSuiteModel.AppId = 1;
        _insertSuiteModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        CommonValidation.InsertTest(field, _suiteValidator, _insertSuiteModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_NameIsEmpty()
    {
        _insertSuiteModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        CommonValidation.InsertTest(field, _suiteValidator, _insertSuiteModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_NameIsWhiteSpace()
    {
        _insertSuiteModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        CommonValidation.InsertTest(field, _suiteValidator, _insertSuiteModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_NameExceedsMaxLength()
    {
        var suite = new Suite();
        var maxLength = suite.GetAttributeFrom<MaxLengthAttribute>("Name");
        var field = new InvalidField("Name", $"exceeds Max Length allowed {maxLength.Length}");
        _insertSuiteModel.Name = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        CommonValidation.InsertTest(field, _suiteValidator, _insertSuiteModel, InsertMessageTitle);
    }

    [TestCase]
    public void ValidateInsert_AppIdZero()
    {
        _insertSuiteModel.AppId = 0;
        var field = new InvalidField("AppId", $"must be greater than zero");

        CommonValidation.InsertTest(field, _suiteValidator, _insertSuiteModel, InsertMessageTitle);
    }


    [TestCase]
    public void ValidateUpdate_Success()
    {
        var suite = new Suite();
        var maxLength = suite.GetAttributeFrom<MaxLengthAttribute>("Name");
        _updateSuiteModel.Name = StringBuilders.BuildStringForLength(maxLength.Length);

        Assert.DoesNotThrow(() => _suiteValidator.ValidateUpdate(_updateSuiteModel));
    }


    [TestCase]
    public void ValidateUpdate_NameIsNull()
    {
        _updateSuiteModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        CommonValidation.UpdateTest(field, _suiteValidator, _updateSuiteModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_NameIsEmpty()
    {
        _updateSuiteModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        CommonValidation.UpdateTest(field, _suiteValidator, _updateSuiteModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_NameIsWhiteSpace()
    {
        _updateSuiteModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        CommonValidation.UpdateTest(field, _suiteValidator, _updateSuiteModel, UpdateMessageTitle);
    }

    [TestCase]
    public void ValidateUpdate_AppIdZero()
    {
        _updateSuiteModel.AppId = 0;
        var field = new InvalidField("AppId", $"must be greater than zero");

        CommonValidation.UpdateTest(field, _suiteValidator, _updateSuiteModel, UpdateMessageTitle);
    }
}