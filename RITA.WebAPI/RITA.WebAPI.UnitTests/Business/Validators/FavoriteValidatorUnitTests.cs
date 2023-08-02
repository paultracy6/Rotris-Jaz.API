using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Business.Validators;
using RITA.WebAPI.Repository.Models;
using ValidationException = RITA.WebAPI.Abstractions.Validation.ValidationException;

namespace RITA.WebAPI.UnitTests.Business.Validators
{
    public class FavoriteValidatorUnitTests
    {
        private readonly IFavoriteValidator _favoriteValidator;
        private IFavoriteModel _favoriteModel = new FavoriteModel();
        private const string _messageTitle = "Favorite Insertion Error";

        public FavoriteValidatorUnitTests()
        {
            _favoriteValidator = new FavoriteValidator();
        }

        [SetUp]
        public void SetUp()
        {
            _favoriteModel.Id = 0;
            _favoriteModel.UserId = "user1";
            _favoriteModel.Name = "fav1";
            _favoriteModel.FavoriteType = "Suite";
            _favoriteModel.ReferenceId = 111;
        }

        [TestCase]
        public void ValidateInsert_Success()
        {
            Assert.DoesNotThrow(() => _favoriteValidator.ValidateInsert(_favoriteModel));
        }

        [TestCase]
        public void ValidateInsert_IdNotZero_Exception()
        {
            var field = new InvalidField("Id", "must be equal zero");
            _favoriteModel.Id = 10;
            InvalidTestHelper(_favoriteValidator, _favoriteModel, field);
        }

        [TestCase]
        public void ValidateInsert_ReferenceIdZero_Exception()
        {
            var field = new InvalidField("ReferenceId", "must be greater than zero");
            _favoriteModel.ReferenceId = 0;
            InvalidTestHelper(_favoriteValidator, _favoriteModel, field);
        }

        [TestCase]
        public void ValidateInsert_UserIdEmpty_Exception()
        {
            var field = new InvalidField("UserId", "cannot be empty");
            _favoriteModel.UserId = string.Empty;
            InvalidTestHelper(_favoriteValidator, _favoriteModel, field);
        }

        [TestCase]
        public void ValidateInsert_NameNull_Exception()
        {
            var field = new InvalidField("Name", "cannot be empty");
            _favoriteModel.Name = string.Empty;
            InvalidTestHelper(_favoriteValidator, _favoriteModel, field);
        }

        [TestCase]
        public void ValidateInsert_FavoriteTypeEmpty_Exception()
        {
            var field = new InvalidField("FavoriteType", "cannot be empty");
            _favoriteModel.FavoriteType = string.Empty;
            InvalidTestHelper(_favoriteValidator, _favoriteModel, field);
        }

        internal void InvalidTestHelper(IFavoriteValidator validator, IFavoriteModel model, InvalidField field)
        {
            var ex = Assert.Throws<ValidationException>(() => validator.ValidateInsert(model));
            Assert.NotNull(ex);
            var result = ex?.InvalidFields;

            var actual = result?.ToArray();

            Assert.NotNull(actual);
            Assert.AreEqual(string.Concat(_messageTitle, ": ", field.Name, "-", field.Message), ex?.GetMessage());
        }
    }
}
