using Moq;
using NUnit.Framework;
using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Utility;
using RITA.WebAPI.Repository.Validators;
using RITA.WebAPI.UnitTests.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ValidationException = RITA.WebAPI.Abstractions.Validation.ValidationException;

namespace RITA.WebAPI.UnitTests.Repository.Validators;

[ExcludeFromCodeCoverage]
public class TestCaseValidatorUnitTests
{
    private TestCaseValidator _testCaseValidator;
    private TestCaseModel _insertTestCaseModel;
    private TestCaseModel _updateTestCaseModel;
    private Mock<IRepositoryValidator> _repositoryValidator;
    private const string InsertMessageTitle = "TestCase Insertion Error";
    private const string UpdateMessageTitle = "TestCase Update Error";

    [SetUp]
    public void TestSetUp()
    {
        _insertTestCaseModel = new TestCaseModel()
        {
            SuiteId = 21,
            Url = "davids-test-blows-up.com",
            Name = "TestCaseInsert",
            RequestMethod = "Get",
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 0,
        };


        _updateTestCaseModel = new TestCaseModel()
        {
            SuiteId = 21,
            Url = "davids-test-blows-up-again.com",
            Name = "TestCaseInsert",
            RequestMethod = "Post",
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 13,
            UpdatedOn = DateTime.Today,
            UpdatedBy = "Paul"
        };

        _repositoryValidator = new Mock<IRepositoryValidator>();
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForInsert(
                It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForUpdate(
                It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());

        _testCaseValidator = new TestCaseValidator(_repositoryValidator.Object);
    }

    [TestCase]
    public void ValidateInsert_Success()
    {
        var testCase = new TestCase();
        var maxLength = testCase.GetAttributeFrom<MaxLengthAttribute>("Name");
        _insertTestCaseModel.Name = StringBuilders.BuildStringForLength(maxLength.Length);

        Assert.DoesNotThrow(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
    }

    [TestCase]
    public void ValidateInsert_SuiteIdZero()
    {
        _insertTestCaseModel.SuiteId = 0;
        var field = new InvalidField("SuiteId", $"must be greater than zero");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_NameIsNull()
    {
        _insertTestCaseModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_NameIsEmpty()
    {
        _insertTestCaseModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_NameIsWhiteSpace()
    {
        _insertTestCaseModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_NameExceedsMaxLength()
    {
        var testCase = new TestCase();
        var maxLength = testCase.GetAttributeFrom<MaxLengthAttribute>("Name");
        var field = new InvalidField("Name", $"exceeds Max Length allowed {maxLength.Length}");
        _insertTestCaseModel.Name = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UrlIsNull()
    {
        _insertTestCaseModel.Url = null;
        var field = new InvalidField("Url", $"must contain a value with at least one character");

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UrlIsEmpty()
    {
        _insertTestCaseModel.Url = string.Empty;
        var field = new InvalidField("Url", $"must contain a value with at least one character");

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UrlIsWhiteSpace()
    {
        _insertTestCaseModel.Url = "     ";
        var field = new InvalidField("Url", $"must contain a value with at least one character");

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UrlExceedsMaxLength()
    {
        var testCase = new TestCase();
        var maxLength = testCase.GetAttributeFrom<MaxLengthAttribute>("Url");
        var field = new InvalidField("Url", $"exceeds Max Length allowed {maxLength.Length}");
        _insertTestCaseModel.Url = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_FailCommonField()
    {
        var field = new InvalidField("CreatedBy", $"must contain a value");
        var fields = new List<IInvalidField>() { field };
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForInsert(
                It.IsAny<ICommonModel>()))
            .Returns(fields);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateInsert(_insertTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }



    [TestCase]
    public void ValidateUpdate_Success()
    {
        var testCase = new TestCase();
        var maxLength = testCase.GetAttributeFrom<MaxLengthAttribute>("Url");
        _updateTestCaseModel.Url = StringBuilders.BuildStringForLength(maxLength.Length);

        Assert.DoesNotThrow(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
    }

    [TestCase]
    public void ValidateUpdate_SuiteIdZero()
    {
        _updateTestCaseModel.SuiteId = 0;
        var field = new InvalidField("SuiteId", $"must be greater than zero");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_NameIsNull()
    {
        _updateTestCaseModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_NameIsEmpty()
    {
        _updateTestCaseModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_NameIsWhiteSpace()
    {
        _updateTestCaseModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_NameExceedsMaxLength()
    {
        var testCase = new TestCase();
        var maxLength = testCase.GetAttributeFrom<MaxLengthAttribute>("Name");
        var field = new InvalidField("Name", $"exceeds Max Length allowed {maxLength.Length}");
        _updateTestCaseModel.Name = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_UrlIsNull()
    {
        _updateTestCaseModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_UrlIsEmpty()
    {
        _updateTestCaseModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_UrlIsWhiteSpace()
    {
        _updateTestCaseModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_UrlExceedsMaxLength()
    {
        var testCase = new TestCase();
        var maxLength = testCase.GetAttributeFrom<MaxLengthAttribute>("Name");
        var field = new InvalidField("Name", $"exceeds Max Length allowed {maxLength.Length}");
        _updateTestCaseModel.Name = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex = Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }

    [TestCase]
    public void ValidateUpdate_FailCommonField()
    {
        var field = new InvalidField("CreatedBy", $"must contain a value");
        var fields = new List<IInvalidField>() { field };
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForUpdate(
                It.IsAny<ICommonModel>()))
            .Returns(fields);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testCaseValidator.ValidateUpdate(_updateTestCaseModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        var actual = result.ToArray();

        Assert.IsNotEmpty(actual);
        Assert.AreEqual(string.Concat(UpdateMessageTitle, ": ", field.Name, "-", field.Message), ex.GetMessage());
    }
}