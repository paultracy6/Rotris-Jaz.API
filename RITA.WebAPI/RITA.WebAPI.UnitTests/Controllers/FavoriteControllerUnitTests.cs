using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Abstractions.WebAPI.Validation;
using RITA.WebAPI.Api.Controllers;
using RITA.WebAPI.Api.Models;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.UnitTests.Utilities;
using RITA.WebAPI.UnitTests.Views;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace RITA.WebAPI.UnitTests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class FavoriteControllerUnitTests
    {
        private readonly Mock<IFavoriteControllerValidator> _validator = new Mock<IFavoriteControllerValidator>();
        private readonly Mock<ILogger> _logger = new Mock<ILogger>();
        private readonly Mock<IFavoriteService> _service = new Mock<IFavoriteService>();
        private readonly FavoritesController _controller;
        private IFavoriteView _view = new FavoriteView();
        private List<IFavoriteView> _views = new List<IFavoriteView>();
        private string _message = string.Empty;

        public FavoriteControllerUnitTests()
        {
            _controller = new FavoritesController(_service.Object, _validator.Object, _logger.Object);
        }

        [SetUp]
        public void TestSetup()
        {
            _validator.Reset();
            _logger.Reset();
            _service.Reset();
            _view.UserId = string.Empty;
            _view.Name = string.Empty;
            _view.FavoriteType = string.Empty;
            _view.ReferenceId = 0;
            _view.Id = 0;
            _views.Clear();
            _message = string.Empty;
        }

        [TestCase]
        public void Favorite_Get_ResponseOk()
        {
            _view.Id = 1;
            _view.Name = "test1";
            _view.FavoriteType = "type1";
            _view.UserId = "user1";
            _view.ReferenceId = 11;
            _views.Add(_view);

            _service
                .Setup(x => x.GetAllByUserId(It.IsAny<string>()))
                .Returns(_views.AsEnumerable<IFavoriteView>);

            IActionResult result = _controller.Get("user1");
            var okResult = (ObjectResult)result;

            Assert.IsInstanceOf<OkObjectResult>(result);
            ComparerUtility.AreEqual(_views, okResult.Value);
        }

        [TestCase]
        public void Favorite_Get_BusinessException()
        {
            _message = "GetAllByUserId BusinessException";
            _service.Setup(x => x.GetAllByUserId(It.IsAny<string>()))
                .Throws(new BusinessException(_message));

            IActionResult result = _controller.Get("user1");
            Assert.IsInstanceOf<ObjectResult>(result);

            var Result = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.BusinessException, Result.StatusCode);
            Assert.AreEqual(_message, Result.Value);
        }

        [TestCase]
        public void Favorite_Get_RepositoryException()
        {
            _message = "GetAllByUserId RepositoryException";
            _service.Setup(x => x.GetAllByUserId(It.IsAny<string>()))
                .Throws(new RepositoryException(_message));

            IActionResult result = _controller.Get("user1");
            Assert.IsInstanceOf<ObjectResult>(result);

            var Result = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.RepositoryException, Result.StatusCode);
            Assert.AreEqual(_message, Result.Value);
        }

        [TestCase]
        public void Favorite_Get_Exception()
        {
            _message = "GetAllByUserId Exception";
            _service.Setup(x => x.GetAllByUserId(It.IsAny<string>()))
                .Throws(new Exception(_message));

            IActionResult result = _controller.Get("user1");
            Assert.IsInstanceOf<ObjectResult>(result);

            var Result = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.ControllerException, Result.StatusCode);
            Assert.AreEqual(_message, Result.Value);
            LoggerUtility.VerifyException(_message, _logger);
        }

        [TestCase]
        public void Favorite_Post_ResponseOk()
        {
            _view.Id = 1;
            _view.Name = "test1";
            _view.FavoriteType = "type1";
            _view.UserId = "user1";
            _view.ReferenceId = 11;

            _service.Setup(x => x.Insert(It.IsAny<IFavoriteView>()))
                .Returns(_view);

            IActionResult result = _controller.Post(_view);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestCase]
        public void Favorite_Post_ValidationException()
        {
            var field = new InvalidField() { Message = "Error", Name = "FieldName" };
            var validationException = new ValidationException("Favorite Data contains invalid fields", new[] { field });
            _service.Setup(x => x.Insert(It.IsAny<IFavoriteView>()))
                 .Throws(validationException);
            IActionResult result = _controller.Post(_view);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var okResult = (ObjectResult)result;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, okResult.StatusCode);
            Assert.AreEqual("Favorite Data contains invalid fields: FieldName-Error", okResult.Value);
        }

        [TestCase]
        public void Favorite_Post_BusinessException()
        {
            _message = "Post BusinessException";

            _service.Setup(x => x.Insert(It.IsAny<IFavoriteView>()))
                .Throws(new BusinessException(_message));

            IActionResult result = _controller.Post(_view);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Favorite_Post_RepositoryException()
        {
            _message = "Post RepositoryException";

            _service.Setup(x => x.Insert(It.IsAny<IFavoriteView>()))
                .Throws(new RepositoryException(_message));

            IActionResult result = _controller.Post(_view);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Favorite_Post_Exception()
        {
            _message = "Post Exception";

            _service.Setup(x => x.Insert(It.IsAny<IFavoriteView>()))
                .Throws(new Exception(_message));

            IActionResult result = _controller.Post(_view);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Favorite_Delete_Success()
        {
            _service.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(1);

            IActionResult result = _controller.Delete(1);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(1, okResult.Value);
        }

        [TestCase]
        public void Favorite_Delete_BusinessException()
        {
            _message = "Delete BussinessException";
            _service.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new BusinessException(_message));

            IActionResult result = _controller.Delete(1);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Favorite_Delete_RepositoryException()
        {
            _message = "Delete RepositoryException";
            _service.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new RepositoryException(_message));

            IActionResult result = _controller.Delete(1);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Favorite_Delete_Exception()
        {
            _message = "Delete Controller Exception";
            _service.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new Exception(_message));

            IActionResult result = _controller.Delete(1);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }
    }
}
