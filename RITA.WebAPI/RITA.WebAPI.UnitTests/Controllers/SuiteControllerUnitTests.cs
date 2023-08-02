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
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using ILogger = RockLib.Logging.ILogger;
using InvalidField = RITA.WebAPI.Api.Models.InvalidField;

namespace RITA.WebAPI.UnitTests.Controllers;

[ExcludeFromCodeCoverage]
[TestFixture]
public class SuiteControllerUnitTests
{
    private Mock<ISuiteControllerValidator> _controllerValidatorMock;
    private Mock<ILogger> _loggerMock;
    private Mock<ISuiteService> _suiteServiceMock;
    private SuiteController _suiteController;
    private Suite _suite;
    private string _message = string.Empty;
    private TestCase _testCase;
    private readonly IEnumerable<TestCase> _testCases = new List<TestCase>();

    [SetUp]
    public void TestSetup()
    {
        _suiteServiceMock = new Mock<ISuiteService>();
        _controllerValidatorMock = new Mock<ISuiteControllerValidator>();
        _loggerMock = new Mock<ILogger>();
        _suiteController = new SuiteController(_suiteServiceMock.Object, _controllerValidatorMock.Object, _loggerMock.Object);
        _suite = new Suite
        {
            Id = 1,
            CreatedOn = new DateTime(2023, 1, 28),
            CreatedBy = "Tester",
            UpdatedOn = new DateTime(2023, 1, 28),
            UpdatedBy = "Tester",
            AppId = 100,
            Name = "Suite1"
        };
        _testCase = new TestCase
        {
            Id = 1,
            CreatedOn = new DateTime(2023, 1, 28),
            CreatedBy = "Tester",
            UpdatedOn = new DateTime(2023, 1, 28),
            UpdatedBy = "Tester",
            SuiteId = 1,
            Url = "TestCaseUrl",
            Name = "TestCase1"
        };
    }

    [TestCase]
    public void Get_ResponseOk()
    {
        _suiteServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(_suite);
        IActionResult result = _suiteController.Get(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_suite), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void Get_BusinessException()
    {
        _message = "GetById BusinessException";
        _suiteServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Throws(new BusinessException(_message));

        IActionResult result = _suiteController.Get(0);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Get_RepositoryException()
    {
        _message = "GetById RepositoryException";
        _suiteServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Throws(new RepositoryException(_message));

        IActionResult result = _suiteController.Get(0);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Get_Exception()
    {
        _message = "GetById Exception";
        _suiteServiceMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Throws(new Exception(_message));

        IActionResult result = _suiteController.Get(0);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Post_ResponseOk()
    {
        _suite.Id = 2;
        _suiteServiceMock.Setup(x => x.Insert(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Returns(_suite);

        IActionResult result = _suiteController.Post(_suite);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_suite), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void Post_ValidationException()
    {
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("Suite Data contains invalid fields", new[] { field });
        _controllerValidatorMock.Setup(x => x.ValidateInsert(It.IsAny<ISuiteView>()))
            .Throws(validationException);

        IActionResult result = _suiteController.Post(_suite);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, okResult.StatusCode);
        Assert.AreEqual("Suite Data contains invalid fields: FieldName-Error", okResult.Value);
    }

    [TestCase]
    public void Post_RepositoryException()
    {
        _message = "Post RepositoryException";
        _suiteServiceMock.Setup(x => x.Insert(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Throws(new RepositoryException(_message));

        IActionResult result = _suiteController.Post(_suite);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Post_BusinessException()
    {
        _message = "Post BusinessException";
        _suiteServiceMock.Setup(x => x.Insert(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Throws(new BusinessException(_message));

        IActionResult result = _suiteController.Post(_suite);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Post_Exception()
    {
        _message = "Post Exception";
        _suiteServiceMock.Setup(x => x.Insert(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Throws(new Exception(_message));

        IActionResult result = _suiteController.Post(_suite);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Put_ResponseOk()
    {
        _suite.Id = 3;
        _suiteServiceMock.Setup(x => x.Update(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Returns(_suite);

        IActionResult result = _suiteController.Put(_suite.Id, _suite);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_suite), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void Put_ValidationException()
    {
        _suite.Id = 3;
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("Suite Data contains invalid fields", new[] { field });
        _controllerValidatorMock.Setup(x => x.ValidateUpdate(It.IsAny<int>(), It.IsAny<ISuiteView>()))
            .Throws(validationException);

        IActionResult result = _suiteController.Put(_suite.Id, _suite);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, okResult.StatusCode);
        Assert.AreEqual("Suite Data contains invalid fields: FieldName-Error", okResult.Value);
    }

    [TestCase]
    public void Put_BusinessException()
    {
        _suite.Id = 3;
        _message = "Put BusinessException";
        _suiteServiceMock.Setup(x => x.Update(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Throws(new BusinessException(_message));

        IActionResult result = _suiteController.Put(_suite.Id, _suite);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Put_RepositoryException()
    {
        _suite.Id = 3;
        _message = "Put RepositoryException";
        _suiteServiceMock.Setup(x => x.Update(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Throws(new RepositoryException(_message));

        IActionResult result = _suiteController.Put(_suite.Id, _suite);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Put_Exception()
    {
        _suite.Id = 3;
        _message = "Put Exception";
        _suiteServiceMock.Setup(x => x.Update(It.IsAny<ISuiteView>(), It.IsAny<string>()))
            .Throws(new Exception(_message));

        IActionResult result = _suiteController.Put(_suite.Id, _suite);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Delete_ResponseOk()
    {
        _suiteServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
        IActionResult result = _suiteController.Delete(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(1, okResult.Value);
    }

    [TestCase]
    public void Delete_Exception()
    {
        _message = "Controller Delete Exception";
        _suiteServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new Exception(_message));

        IActionResult result = _suiteController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Delete_RepositoryException()
    {
        _message = "Delete RepositoryException";
        _suiteServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new RepositoryException(_message));

        IActionResult result = _suiteController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Delete_BusinessException()
    {
        _message = "Delete BusinessException";
        _suiteServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new BusinessException(_message));

        IActionResult result = _suiteController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Suite_GetTestCases_ResponseOk()
    {
        _testCases.Append(_testCase);

        _suiteServiceMock.Setup(x => x.GetTestCasesBySuiteId(It.IsAny<int>())).Returns(_testCases);
        IActionResult result = _suiteController.GetTestCases(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(_testCases, okResult.Value);
    }

    [TestCase]
    public void Suite_GetTestCases_BusinessException()
    {
        _message = "TestCases GetBySuiteId BusinessException";
        _suiteServiceMock.Setup(x => x.GetTestCasesBySuiteId(It.IsAny<int>())).Throws(new BusinessException(_message));

        IActionResult result = _suiteController.GetTestCases(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Suite_GetTestCases_RepositoryException()
    {
        _message = "TestCases GetBySuiteId RepositoryException";
        _suiteServiceMock.Setup(x => x.GetTestCasesBySuiteId(It.IsAny<int>())).Throws(new RepositoryException(_message));

        IActionResult result = _suiteController.GetTestCases(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void Suite_GetTestCases_Exception()
    {
        _message = "TestCases GetBySuiteId Exception";
        _suiteServiceMock.Setup(x => x.GetTestCasesBySuiteId(It.IsAny<int>())).Throws(new Exception(_message));

        IActionResult result = _suiteController.GetTestCases(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

}