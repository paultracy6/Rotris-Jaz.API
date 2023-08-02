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
public class TestDataControllerUnitTests
{
    private Mock<ITestDataControllerValidator> _controllerValidatorMock;
    private Mock<ILogger> _loggerMock;
    private Mock<ITestDataService> _testDataServiceMock;
    private TestDataController _testDataController;
    private TestData _testData;
    private string _message = string.Empty;

    [SetUp]
    public void TestSetup()
    {
        _testDataServiceMock = new Mock<ITestDataService>();
        _controllerValidatorMock = new Mock<ITestDataControllerValidator>();
        _loggerMock = new Mock<ILogger>();
        _testDataController = new TestDataController(_testDataServiceMock.Object, _controllerValidatorMock.Object, _loggerMock.Object);
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
    public void TestData_Get_ResponseOk()
    {
        _testDataServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(_testData);
        IActionResult result = _testDataController.Get(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_testData), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void TestData_Get_BusinessException()
    {
        _message = "GetById BusinessException";
        _testDataServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Throws(new BusinessException(_message));
        IActionResult result = _testDataController.Get(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Get_RepositoryException()
    {
        _message = "GetById RepositoryException";
        _testDataServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Throws(new RepositoryException(_message));
        IActionResult result = _testDataController.Get(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Get_Exception()
    {
        _message = "GetById Exception";
        _testDataServiceMock.Setup(x => x.GetById(It.IsAny<int>())).Throws(new Exception(_message));
        IActionResult result = _testDataController.Get(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void TestData_Post_ResponseOk()
    {
        _testData.Id = 2;
        _testDataServiceMock.Setup(x => x.Insert(It.IsAny<ITestDataView>(), It.IsAny<string>())).Returns(_testData);
        IActionResult result = _testDataController.Post(_testData);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_testData), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void TestData_Post_ValidationException()
    {
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("TestData Data contains invalid fields", new[] { field });
        _controllerValidatorMock.Setup(x => x.ValidateInsert(It.IsAny<ITestDataView>())).Throws(validationException);
        IActionResult result = _testDataController.Post(_testData);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, okResult.StatusCode);
        Assert.AreEqual("TestData Data contains invalid fields: FieldName-Error", okResult.Value);
    }

    [TestCase]
    public void TestData_Post_RepositoryException()
    {
        _message = "Post RepositoryException";
        _testDataServiceMock.Setup(x => x.Insert(It.IsAny<ITestDataView>(), It.IsAny<string>())).Throws(new RepositoryException(_message));
        IActionResult result = _testDataController.Post(_testData);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Post_BusinessException()
    {
        _message = "Post BusinessException";
        _testDataServiceMock.Setup(x => x.Insert(It.IsAny<ITestDataView>(), It.IsAny<string>())).Throws(new BusinessException(_message));
        IActionResult result = _testDataController.Post(_testData);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Post_Exception()
    {
        _message = "Post Exception";
        _testDataServiceMock.Setup(x => x.Insert(It.IsAny<ITestDataView>(), It.IsAny<string>())).Throws(new Exception(_message));
        IActionResult result = _testDataController.Post(_testData);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void TestData_Put_ResponseOk()
    {
        _testData.Id = 3;
        _testDataServiceMock.Setup(x => x.Update(It.IsAny<ITestDataView>(), It.IsAny<string>())).Returns(_testData);
        IActionResult result = _testDataController.Put(_testData.Id, _testData);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);

        PropertyInfo[] propertyInfo = okResult.Value?.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo!)
        {
            Assert.AreEqual(property.GetValue(_testData), property.GetValue(okResult.Value));
        }
    }

    [TestCase]
    public void TestData_Put_ValidationException()
    {
        _testData.Id = 3;
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("TestData Data contains invalid fields", new[] { field });
        _controllerValidatorMock.Setup(x => x.ValidateUpdate(It.IsAny<int>(), It.IsAny<ITestDataView>())).Throws(validationException);
        IActionResult result = _testDataController.Put(_testData.Id, _testData);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, okResult.StatusCode);
        Assert.AreEqual("TestData Data contains invalid fields: FieldName-Error", okResult.Value);
    }

    [TestCase]
    public void TestData_Put_BusinessException()
    {
        _testData.Id = 3;
        _message = "Put BusinessException";
        _testDataServiceMock.Setup(x => x.Update(It.IsAny<ITestDataView>(), It.IsAny<string>())).Throws(new BusinessException(_message));
        IActionResult result = _testDataController.Put(_testData.Id, _testData);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Put_RepositoryException()
    {
        _testData.Id = 3;
        _message = "Put RepositoryException";
        _testDataServiceMock.Setup(x => x.Update(It.IsAny<ITestDataView>(), It.IsAny<string>())).Throws(new RepositoryException(_message));
        IActionResult result = _testDataController.Put(_testData.Id, _testData);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Put_Exception()
    {
        _testData.Id = 3;
        _message = "Put Exception";
        _testDataServiceMock.Setup(x => x.Update(It.IsAny<ITestDataView>(), It.IsAny<string>())).Throws(new Exception(_message));
        IActionResult result = _testDataController.Put(_testData.Id, _testData);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void TestData_Delete_ResponseOk()
    {
        _testDataServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(1);
        IActionResult result = _testDataController.Delete(1);
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(1, okResult.Value);
    }

    [TestCase]
    public void TestData_Delete_RepositoryException()
    {
        _message = "Delete RepositoryException";
        _testDataServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new RepositoryException(_message));
        IActionResult result = _testDataController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.RepositoryException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Delete_BusinessException()
    {
        _message = "Delete BusinessException";
        _testDataServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new BusinessException(_message));
        IActionResult result = _testDataController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;

        Assert.AreEqual((int)StatusCodes.BusinessException, okResult.StatusCode);
        Assert.AreEqual(_message, okResult.Value);
    }

    [TestCase]
    public void TestData_Delete_Exception()
    {
        _message = "Controller Delete Exception";
        _testDataServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new Exception(_message));
        IActionResult result = _testDataController.Delete(1);
        Assert.IsInstanceOf<ObjectResult>(result);

        var okResult = (ObjectResult)result;
        Assert.AreEqual((int)StatusCodes.ControllerException, okResult.StatusCode);
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

}