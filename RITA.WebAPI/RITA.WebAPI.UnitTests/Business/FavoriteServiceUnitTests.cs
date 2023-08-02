using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Converters;
using RITA.WebAPI.Business.Services;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.UnitTests.Models;
using RITA.WebAPI.UnitTests.Utilities;
using RITA.WebAPI.UnitTests.Views;
using RockLib.Logging;

namespace RITA.WebAPI.UnitTests.Business
{
    public class FavoriteServiceUnitTests
    {
        private readonly Mock<IFavoriteRepository> _favoriteRepository;
        private readonly Mock<IFavoriteValidator> _favoriteValidator;
        private readonly Mock<ILogger> _logger;
        private readonly IFavoriteService _favoriteService;
        private readonly IFavoriteView _favoriteView = new FavoriteView();
        private readonly IFavoriteModel _favoriteModel = new FavoriteModel();
        private readonly List<IFavoriteModel> _favorites = new();

        public FavoriteServiceUnitTests()
        {
            _favoriteRepository = new Mock<IFavoriteRepository>();
            _favoriteValidator = new Mock<IFavoriteValidator>();
            _logger = new Mock<ILogger>();
            _favoriteService = new FavoriteService(_favoriteRepository.Object, _favoriteValidator.Object, _logger.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _favoriteRepository.Reset();
            _favoriteValidator.Reset();
            _logger.Reset();
            _favoriteView.Id = 0;
            _favoriteView.UserId = string.Empty;
            _favoriteView.Name = string.Empty;
            _favoriteView.ReferenceId = 0;
            _favoriteView.FavoriteType = string.Empty;
            _favorites.Clear();
        }

        [TestCase]
        public void GetAllByUserID_Success()
        {
            _favoriteModel.Id = 10;
            _favoriteModel.UserId = "user1";
            _favoriteModel.Name = "case1";
            _favoriteModel.ReferenceId = 100;
            _favoriteModel.FavoriteType = "suite";
            _favorites.Add(_favoriteModel);

            _favoriteRepository.Setup(x => x.GetFavoritesByUserId(It.IsAny<string>()))
                .Returns(_favorites);

            ComparerUtility.AreEqual(_favorites.ToViews(), _favoriteService.GetAllByUserId("user1"));
        }

        [TestCase]
        public void GetAllByUserId_Exception()
        {
            var message = "GetAllByUserId Exception";
            _favoriteRepository.Setup(x => x.GetFavoritesByUserId(It.IsAny<string>()))
                .Throws(new Exception(message));

            Assert.Throws<Exception>(() => _favoriteService.GetAllByUserId("user1"));
            LoggerUtility.VerifyException(message, _logger);
        }

        [TestCase]
        public void Inset_Success()
        {
            _favoriteModel.Id = 10;
            _favoriteModel.UserId = "user1";
            _favoriteModel.Name = "case1";
            _favoriteModel.ReferenceId = 100;
            _favoriteModel.FavoriteType = "suite";
            _favoriteRepository.Setup(x => x.Insert(It.IsAny<IFavoriteModel>()))
                .Returns(_favoriteModel);

            var result = _favoriteService.Insert(_favoriteView);

            Assert.AreEqual(_favoriteModel.Id, result.Id);
        }

        [TestCase]
        public void Insert_ValidationException()
        {
            var validationException = new ValidationException("BAD DATA", Array.Empty<IInvalidField>());
            _favoriteValidator.Setup(x => x.ValidateInsert(It.IsAny<IFavoriteModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _favoriteService.Insert(_favoriteView));
        }

        [TestCase]
        public void Insert_Exception()
        {
            _favoriteRepository.Setup(x => x.Insert(It.IsAny<IFavoriteModel>()))
                .Throws(new Exception());

            Assert.Throws<BusinessException>(() => _favoriteService.Insert(_favoriteView));
        }

        [TestCase]
        public void Delete_Success()
        {
            _favoriteRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(1);

            Assert.AreEqual(1, _favoriteService.Delete(1));
        }

        [TestCase]
        public void Delete_Exception()
        {
            var message = "Test Delete Exception";
            _favoriteRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new Exception(message));

            Assert.Throws<BusinessException>(() => _favoriteService.Delete(10));

            LoggerUtility.VerifyException(message, _logger);
        }
    }
}
