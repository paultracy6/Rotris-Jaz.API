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
public class TestCasesControllerUnitTests
{
    private Mock<ITestCaseControllerValidator> _controllerValidatorMock;
    private Mock<ILogger> _loggerMock;
    private Mock<ITestCaseService> _testCaseServiceMock;
    private TestCasesController _testCasesController;
    private TestCase _testCase;
    private string _message = string.Empty;
    private TestData _testData;
    private IEnumerable<TestData> _testDatas = new List<TestData>();


    [SetUp]
    public void TestSetup()
    {
        _testCaseServiceMock = new Mock<ITestCaseService>();
        _controllerValidatorMock = new Mock<ITestCaseControllerValidator>();
        _loggerMock = new Mock<ILogger>();
        _testCasesController = new TestCasesController(_testCaseServiceMock.Object, _controllerValidatorMock.Object, _loggerMock.Object);
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
        _testData = new TestData
        {
            Id = 1,
            CreatedOn = new DateTime(2023, 1, 28),
            CreatedBy = "Tester",
            UpdatedOn = new DateTime(2023, 1, 28),
            UpdatedBy = "Tester",
            TestTypeId = 1,
            TestCaseId = 1,
            Name = "TestData1",
            IsDefault = false,
            IsSuspended = false,
            SuspendedOn = new DateTime(2023, 1, 28),
            SuspendedBy = "Testter1",
            Comment = "Comment Good"
        };

    }

    [TestCase]
    public void TestCase_Get_ResponseOk()
    {
        _testCaseServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(_testCase);
        IActionResult result = _testCasesController.Get(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_testCase), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void TestCase_Get_BusinessException()
    {
        _message = "GetById BusinessException";
        _testCaseServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Throws(new BusinessException(_message));
        IActionResult result = _testCasesController.Get(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Get_RepositoryException()
    {
        _message = "GetById RepositoryException";
        _testCaseServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Throws(new RepositoryException(_message));
        IActionResult result = _testCasesController.Get(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Get_Exception()
    {
        _message = "GetById Exception";
        _testCaseServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Throws(new Exception(_message));
        IActionResult result = _testCasesController.Get(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);

    }

    [TestCase]
    public void TestCase_Post_ResponseOk()
    {
        _testCase.Id = 2;
        _testCaseServiceMock.Setup(x => x.Insert(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Returns(_testCase);
        IActionResult result = _testCasesController.Post(_testCase);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_testCase), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void TestCase_Post_ValidationException()
    {
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("TestCase Data contains invalid fields", new[] { field });
        _controllerValidatorMock.Setup(x => x.ValidateInsert(It.IsAny<ITestCaseView>())).Throws(validationException);
        IActionResult result = _testCasesController.Post(_testCase);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, okResult.StatusCode);
        Assert.AreEqual("TestCase Data contains invalid fields: FieldName-Error", okResult.Value);
    }

    [TestCase]
    public void TestCase_Post_RepositoryException()
    {
        _message = "Post RepositoryException";
        _testCaseServiceMock.Setup(x => x.Insert(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Throws(new RepositoryException(_message));
        IActionResult result = _testCasesController.Post(_testCase);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Post_BusinessException()
    {
        _message = "Post BusinessException";
        _testCaseServiceMock.Setup(x => x.Insert(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Throws(new BusinessException(_message));
        IActionResult result = _testCasesController.Post(_testCase);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Post_Exception()
    {
        _message = "Post Exception";
        _testCaseServiceMock.Setup(x => x.Insert(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Throws(new Exception(_message));
        IActionResult result = _testCasesController.Post(_testCase);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void TestCase_Put_ResponseOk()
    {
        _testCase.Id = 3;
        _testCaseServiceMock.Setup(x => x.Update(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Returns(_testCase);
        IActionResult result = _testCasesController.Put(_testCase.Id, _testCase);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_testCase), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void TestCase_Put_ValidationException()
    {
        _testCase.Id = 3;
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("TestCase Data contains invalid fields", new[] { field });
        _controllerValidatorMock.Setup(x => x.ValidateUpdate(It.IsAny<int>(), It.IsAny<ITestCaseView>())).Throws(validationException);
        IActionResult result = _testCasesController.Put(_testCase.Id, _testCase);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, okResult.StatusCode);
        Assert.AreEqual("TestCase Data contains invalid fields: FieldName-Error", okResult.Value);
    }

    [TestCase]
    public void TestCase_Put_BusinessException()
    {
        _testCase.Id = 3;
        _message = "Put BusinessException";
        _testCaseServiceMock.Setup(x => x.Update(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Throws(new BusinessException(_message));
        IActionResult result = _testCasesController.Put(_testCase.Id, _testCase);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Put_RepositoryException()
    {
        _testCase.Id = 3;
        _message = "Put RepositoryException";
        _testCaseServiceMock.Setup(x => x.Update(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Throws(new RepositoryException(_message));

        IActionResult result = _testCasesController.Put(_testCase.Id, _testCase);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Put_Exception()
    {
        _testCase.Id = 3;
        _message = "Put Exception";
        _testCaseServiceMock.Setup(x => x.Update(It.IsAny<ITestCaseView>(), It.IsAny<string>())).Throws(new Exception(_message));

        IActionResult result = _testCasesController.Put(_testCase.Id, _testCase);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void TestCase_Delete_ResponseOk()
    {
        _testCaseServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
        IActionResult result = _testCasesController.Delete(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(1, okResult.Value);
    }

    [TestCase]
    public void TestCase_Delete_RepositoryException()
    {
        _message = "Delete RepositoryException";
        _testCaseServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new RepositoryException(_message));
        IActionResult result = _testCasesController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Delete_BusinessException()
    {
        _message = "Delete BusinessException";
        _testCaseServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new BusinessException(_message));
        IActionResult result = _testCasesController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_Delete_Exception()
    {
        _message = "Controller Delete Exception";
        _testCaseServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new Exception(_message));
        IActionResult result = _testCasesController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void TestCase_GetTestData_ResponseOk()
    {
        _testDatas.Append(_testData);
        _testCaseServiceMock.Setup(x => x.GetTestData(It.IsAny<int>())).Returns(_testDatas);
        IActionResult result = _testCasesController.GetTestData(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(_testDatas, okResult.Value);
    }

    [TestCase]
    public void TestCase_GetTestData_BusinessException()
    {
        _message = "TestData GetByTestCaseId BusinessException";
        _testCaseServiceMock.Setup(x => x.GetTestData(It.IsAny<int>())).Throws(new BusinessException(_message));
        IActionResult result = _testCasesController.GetTestData(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_GetTestData_RepositoryException()
    {
        _message = "TestData GetByTestCaseId RepositoryException";
        _testCaseServiceMock.Setup(x => x.GetTestData(It.IsAny<int>())).Throws(new RepositoryException(_message));
        IActionResult result = _testCasesController.GetTestData(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestCase_GetTestData_Exception()
    {
        _message = "TestData GetByTestCaseId Exception";
        _testCaseServiceMock.Setup(x => x.GetTestData(It.IsAny<int>())).Throws(new Exception(_message));
        IActionResult result = _testCasesController.GetTestData(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

}