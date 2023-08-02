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
using ValidationException = RITA.WebAPI.Abstractions.Validation.ValidationException;

namespace RITA.WebAPI.UnitTests.Repository.Validators;

public class TestDataRepositoryValidatorUnitTests
{
    private Mock<IRepositoryValidator> _repositoryValidator;
    private TestDataModel _insertTestDataModel;
    private TestDataModel _updateTestDataModel;
    private TestDataValidator _testDataValidator;

    [SetUp]
    public void TestSetUp()
    {
        _insertTestDataModel = new TestDataModel()
        {
            TestCaseId = 4,
            TestTypeId = 1,
            Comment = "",
            IsSuspended = false,
            SuspendedBy = "",
            SuspendedOn = null,
            Name = "TestDataInsert",
            CreatedBy = "Jazz",
            CreatedOn = DateTime.Today,
            Id = 0,
        };

        _updateTestDataModel = new TestDataModel()
        {
            TestCaseId = 4,
            TestTypeId = 1,
            Comment = "", // Length only
            IsSuspended = false,
            SuspendedBy = "", // Length Only
            SuspendedOn = null,  // Valid Date type
            Name = "TestDataUpdate",
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

        _testDataValidator = new TestDataValidator(_repositoryValidator.Object);
    }

    [TestCase]
    public void ValidateInsert_Success()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("Name");
        _insertTestDataModel.Name = StringBuilders.BuildStringForLength(maxLength.Length);

        Assert.DoesNotThrow(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
    }

    // This test is testing two failures to ensure that the MessageBuilder is working properly 
    // When more than one Validation Error is present in the object
    [TestCase]
    public void ValidateInsert_TestCaseIdZero_FailCommonField()
    {
        _insertTestDataModel.TestCaseId = 0;
        var field = new InvalidField("CreatedBy", $"must contain a value");
        var field2 = new InvalidField("TestCaseId", $"must be greater than zero");

        var fields = new List<IInvalidField>() { field };
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForInsert(
                It.IsAny<ICommonModel>()))
            .Returns(fields);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        Assert.Multiple(() =>
         {
             ComparerUtility.AreEqual(field, result[0]);
             ComparerUtility.AreEqual(field2, result[1]);
         });

    }

    [TestCase]
    public void ValidateInsert_TestTypeIdZero()
    {
        _insertTestDataModel.TestTypeId = 0;
        var field = new InvalidField("TestTypeId", $"must be greater than zero");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateInsert_NameIsNull()
    {
        _insertTestDataModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateInsert_NameIsEmpty()
    {
        _insertTestDataModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateInsert_NameIsWhiteSpace()
    {
        _insertTestDataModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateInsert_NameExceedsMaxLength()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("Name");
        var field = new InvalidField("Name", $"exceeds Max Length allowed {maxLength.Length}");
        _insertTestDataModel.Name = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateInsert_CommentExceedsMaxLength()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("Comment");
        var field = new InvalidField("Comment", $"exceeds Max Length allowed {maxLength.Length}");
        _insertTestDataModel.Comment = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateInsert_SuspendedByExceedsMaxLength()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("SuspendedBy");
        var field = new InvalidField("SuspendedBy", $"exceeds Max Length allowed {maxLength.Length}");
        _insertTestDataModel.SuspendedBy = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateInsert_SuspendedOn_Future()
    {
        _insertTestDataModel.SuspendedOn = DateTime.MaxValue;
        var field = new InvalidField("SuspendedOn", $"must be on or before the current date");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateInsert(_insertTestDataModel));

        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }



    [TestCase]
    public void ValidateUpdate_Success()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("Name");
        _updateTestDataModel.Name = StringBuilders.BuildStringForLength(maxLength.Length);

        Assert.DoesNotThrow(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
    }

    [TestCase]
    public void ValidateUpdate_TestCaseIdZero()
    {
        _updateTestDataModel.TestCaseId = 0;
        var field = new InvalidField("TestCaseId", $"must be greater than zero");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_TestTypeIdZero()
    {
        _updateTestDataModel.TestTypeId = 0;
        var field = new InvalidField("TestTypeId", $"must be greater than zero");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_NameIsNull()
    {
        _updateTestDataModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_NameIsEmpty()
    {
        _updateTestDataModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_NameIsWhiteSpace()
    {
        _updateTestDataModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_NameExceedsMaxLength()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("Name");
        var field = new InvalidField("Name", $"exceeds Max Length allowed {maxLength.Length}");
        _updateTestDataModel.Name = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_CommentExceedsMaxLength()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("Comment");
        var field = new InvalidField("Comment", $"exceeds Max Length allowed {maxLength.Length}");
        _updateTestDataModel.Comment = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_SuspendedByExceedsMaxLength()
    {
        var testData = new TestData();
        var maxLength = testData.GetAttributeFrom<MaxLengthAttribute>("SuspendedBy");
        var field = new InvalidField("SuspendedBy", $"exceeds Max Length allowed {maxLength.Length}");
        _updateTestDataModel.SuspendedBy = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }

    [TestCase]
    public void ValidateUpdate_SuspendedOn_Future()
    {
        _updateTestDataModel.SuspendedOn = DateTime.MaxValue;
        var field = new InvalidField("SuspendedOn", $"must be on or before the current date");

        ValidationException ex =
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));

        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
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
            Assert.Throws<ValidationException>(() => _testDataValidator.ValidateUpdate(_updateTestDataModel));
        Assert.NotNull(ex);
        IList<IInvalidField> result = ex.InvalidFields;

        Assert.IsNotEmpty(result);
        ComparerUtility.AreEqual(field, result[0]);
    }
}