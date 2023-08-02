using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Api.Controllers;
using RITA.WebAPI.Core.Models;
using System.Diagnostics.CodeAnalysis;
using ILogger = RockLib.Logging.ILogger;

namespace RITA.WebAPI.UnitTests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ApplicationControllerUnitTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IApplicationService> _applicationServiceMock;
        private readonly ApplicationController _controller;
        private readonly IEnumerable<ISuiteView> _suites = new List<ISuiteView>();
        private readonly IEnumerable<ITestCaseView> _testCases = new List<ITestCaseView>();

        private string _message = string.Empty;

        public ApplicationControllerUnitTests()
        {
            _applicationServiceMock = new Mock<IApplicationService>();
            _loggerMock = new Mock<ILogger>();
            _controller = new ApplicationController(_applicationServiceMock.Object, _loggerMock.Object);
        }

        [TestCase]
        public void Application_GetSuites_ResponseOk()
        {
            _applicationServiceMock.Setup(x => x.GetSuiteByAppId(It.IsAny<int>())).Returns(_suites);

            IActionResult result = _controller.GetSuites(1);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (ObjectResult)result;

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(_suites, okResult.Value);
        }

        [TestCase]
        public void Application_GetSuites_BusinessException()
        {
            _message = "Suites GetByAppId BusinessException";
            _applicationServiceMock.Setup(x => x.GetSuiteByAppId(It.IsAny<int>())).Throws(new BusinessException(_message));

            IActionResult result = _controller.GetSuites(0);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;

            Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Application_GetSuites_RepositoryException()
        {
            _message = "Suites GetByAppId RepositoryException";
            _applicationServiceMock.Setup(x => x.GetSuiteByAppId(It.IsAny<int>())).Throws(new RepositoryException(_message));

            IActionResult result = _controller.GetSuites(0);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;

            Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Application_GetSuites_Exception()
        {
            _message = "Suites GetByAppId Exception";
            _applicationServiceMock.Setup(x => x.GetSuiteByAppId(It.IsAny<int>())).Throws(new Exception(_message));

            IActionResult result = _controller.GetSuites(0);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Application_GetTestCases_ResponseOk()
        {
            _applicationServiceMock.Setup(x => x.GetTestCaseByAppId(It.IsAny<int>())).Returns(_testCases);

            IActionResult result = _controller.GetTestCases(1);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = (ObjectResult)result;

            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(_testCases, okResult.Value);
        }

        [TestCase]
        public void Application_GetTestCases_BusinessException()
        {
            _message = "Application Controller GetByAppId BusinessException";
            _applicationServiceMock.Setup(x => x.GetTestCaseByAppId(It.IsAny<int>())).Throws(new BusinessException(_message));

            IActionResult result = _controller.GetTestCases(0);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;

            Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Application_GetTestCases_RepositoryException()
        {
            _message = "Application Controller GetByAppId RepositoryException";
            _applicationServiceMock.Setup(x => x.GetTestCaseByAppId(It.IsAny<int>())).Throws(new RepositoryException(_message));

            IActionResult result = _controller.GetTestCases(0);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;

            Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
            Assert.AreEqual(_message, okResult.Value);
        }

        [TestCase]
        public void Application_GetTestCases_Exception()
        {
            _message = "Application Controller GetByAppId Exception";
            _applicationServiceMock.Setup(x => x.GetTestCaseByAppId(It.IsAny<int>())).Throws(new Exception(_message));

            IActionResult result = _controller.GetTestCases(0);
            Assert.IsInstanceOf<ObjectResult>(result);

            var okResult = (ObjectResult)result;
            Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }
    }
}
