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
public class FavoriteValidatorUnitTests
{

    private FavoriteModel _insertFavoriteModel = new FavoriteModel();
    private Mock<IRepositoryValidator> _repositoryValidator = new Mock<IRepositoryValidator>();
    private FavoriteValidator _favoriteValidator;
    private const string InsertMessageTitle = "Favorite Insertion Error";

    [SetUp]

    public void TestSetup()
    {
        _insertFavoriteModel = new FavoriteModel()
        {
            Id = 0,
            UserId = "user1",
            Name = "favorite1",
            FavoriteType = "T1",
            ReferenceId = 11
        };

        _repositoryValidator.Reset();
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForInsert(
                It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());
        _repositoryValidator.Setup(x => x.InvalidCommonFieldsForUpdate(
                It.IsAny<ICommonModel>()))
            .Returns(new List<IInvalidField>());

        _favoriteValidator = new FavoriteValidator(_repositoryValidator.Object);
    }

    [TestCase]
    public void ValidateInsert_Success()
    {
        var favorite = new Favorite();
        var maxLength = favorite.GetAttributeFrom<MaxLengthAttribute>("Name");
        _insertFavoriteModel.Name = StringBuilders.BuildStringForLength(maxLength.Length);

        Assert.DoesNotThrow(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
    }

    [TestCase]
    public void ValidateInsert_NameIsNull()
    {
        _insertFavoriteModel.Name = null;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_NameIsEmpty()
    {
        _insertFavoriteModel.Name = string.Empty;
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_NameIsWhiteSpace()
    {
        _insertFavoriteModel.Name = "     ";
        var field = new InvalidField("Name", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_NameExceedsMaxlength()
    {
        var favorite = new Favorite();
        var maxLength = favorite.GetAttributeFrom<MaxLengthAttribute>("Name");
        var field = new InvalidField("Name", $"exceeds Max Length allowed {maxLength.Length}");
        _insertFavoriteModel.Name = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UserIdIsNull()
    {
        _insertFavoriteModel.UserId = null;
        var field = new InvalidField("UserId", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UserIdIsEmpty()
    {
        _insertFavoriteModel.UserId = string.Empty;
        var field = new InvalidField("UserId", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UserIdIsWhiteSpace()
    {
        _insertFavoriteModel.UserId = "     ";
        var field = new InvalidField("UserId", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_UserIdExceedsMaxlength()
    {
        var favorite = new Favorite();
        var maxLength = favorite.GetAttributeFrom<MaxLengthAttribute>("UserId");
        var field = new InvalidField("UserId", $"exceeds Max Length allowed {maxLength.Length}");
        _insertFavoriteModel.UserId = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_FavoriteTypeIsNull()
    {
        _insertFavoriteModel.FavoriteType = null;
        var field = new InvalidField("FavoriteType", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_FavoriteTypeIsEmpty()
    {
        _insertFavoriteModel.FavoriteType = string.Empty;
        var field = new InvalidField("FavoriteType", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_FavoriteTypeIsWhiteSpace()
    {
        _insertFavoriteModel.FavoriteType = "     ";
        var field = new InvalidField("FavoriteType", $"must contain a value with at least one character");

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    [TestCase]
    public void ValidateInsert_FavoriteTypeExceedsMaxlength()
    {
        var favorite = new Favorite();
        var maxLength = favorite.GetAttributeFrom<MaxLengthAttribute>("FavoriteType");
        var field = new InvalidField("FavoriteType", $"exceeds Max Length allowed {maxLength.Length}");
        _insertFavoriteModel.FavoriteType = StringBuilders.BuildStringForLength(maxLength.Length + 1);

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

    public void ValidateInsert_ReferenceIdIsZero()
    {
        var field = new InvalidField("ReferenceId", "is zero");

        _insertFavoriteModel.ReferenceId = 0;

        var ex =
            Assert.Throws<ValidationException>(() => _favoriteValidator.ValidateInsert(_insertFavoriteModel));
        Assert.NotNull(ex);
        var result = ex?.InvalidFields;

        var actual = result?.ToArray();

        Assert.NotNull(actual);
        Assert.AreEqual(string.Concat(InsertMessageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
    }

}
