using NUnit.Framework;
using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Utility;
using RITA.WebAPI.Repository.Validators;
using RITA.WebAPI.UnitTests.Utilities;
using System.ComponentModel.DataAnnotations;
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;

namespace RITA.WebAPI.UnitTests.Repository.Validators;

public class RepositoryValidatorUnitTests
{
    private readonly RepositoryValidator _repositoryValidator;
    private SuiteModel _insertModel;
    private SuiteModel _updateModel;

    public RepositoryValidatorUnitTests()
    {
        BuildData();
        _repositoryValidator = new RepositoryValidator();
    }

    [SetUp]
    public void TestSetUp()
    {
        _insertModel = new SuiteModel()
        {
            AppId = 1,
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 0,
            Name = "Jazzy"
        };

        _updateModel = new SuiteModel()
        {
            AppId = 1,
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 4,
            Name = "Jazzy",
            UpdatedBy = "PaulBeTheOne",
            UpdatedOn = DateTime.Today
        };
    }

    [TestCase]
    public void ValidateInsert_IdNotZero()
    {
        _insertModel.Id = 7;
        var field = new InvalidField("Id", "must equal zero");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }


    [TestCase]
    public void ValidateInsert_CreatedOnNotCurrent_Previous()
    {
        _insertModel.CreatedOn = DateTime.Parse("2020,01,01");
        var field = new InvalidField("CreatedOn", "must be the current date");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedOnNotCurrent_Future()
    {
        _insertModel.CreatedOn = DateTime.MaxValue;
        var field = new InvalidField("CreatedOn", $"must be the current date");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedByExceedsMaxLength()
    {
        var suite = new Suite();
        var maxLength = suite.GetAttributeFrom<MaxLengthAttribute>("CreatedBy");
        var field = new InvalidField("CreatedBy", $"exceeds Max Length allowed {maxLength.Length}");
        _insertModel.CreatedBy = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedByIsNull()
    {
        _insertModel.CreatedBy = null;
        var field = new InvalidField("CreatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedByIsEmpty()
    {
        _insertModel.CreatedBy = string.Empty;
        var field = new InvalidField("CreatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateInsert_CreatedByIsWhiteSpace()
    {
        _insertModel.CreatedBy = "     ";
        var field = new InvalidField("CreatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateInsert_UpdatedOnHasDate()
    {
        _insertModel.UpdatedOn = DateTime.Today;
        var field = new InvalidField("UpdatedOn", $"must not contain a date");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateInsert_UpdatedByHasValue()
    {
        _insertModel.UpdatedBy = "PaulDidThis";
        var field = new InvalidField("UpdatedBy", $"must not contain a value");

        var result = _repositoryValidator.InvalidCommonFieldsForInsert(_insertModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_IdZero()
    {
        _updateModel.Id = 0;
        var field = new InvalidField("Id", $"must be greater than zero");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_CreatedOn_Future()
    {
        _updateModel.CreatedOn = DateTime.MaxValue;
        var field = new InvalidField("CreatedOn", $"must be on or before the current date");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_CreatedByExceedsMaxLength()
    {
        var suite = new Suite();
        var maxLength = suite.GetAttributeFrom<MaxLengthAttribute>("CreatedBy");
        var field = new InvalidField("CreatedBy", $"exceeds Max Length allowed {maxLength.Length}");
        _updateModel.CreatedBy = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_CreatedByIsNull()
    {
        _updateModel.CreatedBy = null;
        var field = new InvalidField("CreatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_CreatedByIsEmpty()
    {
        _updateModel.CreatedBy = string.Empty;
        var field = new InvalidField("CreatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_CreatedByIsWhiteSpace()
    {
        _updateModel.CreatedBy = "     ";
        var field = new InvalidField("CreatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedOn_Future()
    {
        _updateModel.UpdatedOn = DateTime.MaxValue;
        var field = new InvalidField("UpdatedOn", $"must be the current date");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedOn_Previous()
    {
        _updateModel.UpdatedOn = DateTime.Parse("2020,01,01");
        var field = new InvalidField("UpdatedOn", $"must be the current date");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedByExceedsMaxLength()
    {
        var suite = new Suite();
        var maxLength = suite.GetAttributeFrom<MaxLengthAttribute>("UpdatedBy");
        var field = new InvalidField("UpdatedBy", $"exceeds Max Length allowed {maxLength.Length}");
        _updateModel.UpdatedBy = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedByIsNull()
    {
        _updateModel.UpdatedBy = null;
        var field = new InvalidField("UpdatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedByIsEmpty()
    {
        _updateModel.UpdatedBy = string.Empty;
        var field = new InvalidField("UpdatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    [TestCase]
    public void ValidateUpdate_UpdatedByIsWhiteSpace()
    {
        _updateModel.UpdatedBy = "     ";
        var field = new InvalidField("UpdatedBy", $"must contain a value with at least one character");

        var result = _repositoryValidator.InvalidCommonFieldsForUpdate(_updateModel);

        Assert.NotNull(result);
        var actual = result.ToList();

        ValidateActualErrorList(actual, field);
    }

    private static void ValidateActualErrorList(IReadOnlyList<IInvalidField> actual, IInvalidField field)
    {
        Assert.Multiple(() =>
        {
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(field.Name, actual[0].Name);
            Assert.AreEqual(field.Message, actual[0].Message);
        });
    }
}