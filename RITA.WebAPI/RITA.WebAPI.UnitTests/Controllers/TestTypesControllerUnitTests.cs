using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Api.Controllers;
using RITA.WebAPI.Api.Models;
using RITA.WebAPI.Core.Models;
using System.Diagnostics.CodeAnalysis;
using ILogger = RockLib.Logging.ILogger;

namespace RITA.WebAPI.UnitTests.Controllers;

[ExcludeFromCodeCoverage]
[TestFixture]
public class TestTypesControllerUnitTests
{
	private Mock<ILogger> _loggerMock;
	private Mock<ITestTypeService> _testTypeServiceMock;
	private TestTypesController _testTypeController;
	private TestType _testType;
	private IEnumerable<TestType> _testTypes = new List<TestType>();
	private string _message = string.Empty;


	[SetUp]
	public void TestSetup()
	{
		_testTypeServiceMock = new Mock<ITestTypeService>();
		_loggerMock = new Mock<ILogger>();
		_testTypeController = new TestTypesController(_testTypeServiceMock.Object,_loggerMock.Object);
		_testType = new TestType
		{
			Id = 1,
			CreatedOn = new DateTime(2023,1,28),
			CreatedBy = "Tester",
			UpdatedOn = new DateTime(2023,1,28),
			UpdatedBy = "Tester",
			Name = "TestType1"
		};
	}

	[TestCase]
	public void TestType_GetTestTypeAll_ResponseOk()
	{
		_testTypes.Append(_testType);
		_testTypeServiceMock.Setup(x => x.GetAll()).Returns(_testTypes);
		IActionResult result = _testTypeController.GetTestTypeAll();
		Assert.IsInstanceOf<OkObjectResult>(result);

		var okResult = (ObjectResult)result;

		Assert.AreEqual(200,okResult.StatusCode);
		Assert.AreEqual(_testTypes,okResult.Value);
	}

	[TestCase]
	public void TestType_GetTestTypeAll_BusinessException()
	{
		_testTypes.Append(_testType);
		_message = "GetById BusinessException";
		_testTypeServiceMock.Setup(x => x.GetAll()).Throws(new BusinessException(_message));
		IActionResult result = _testTypeController.GetTestTypeAll();
		Assert.IsInstanceOf<ObjectResult>(result);

		var okResult = (ObjectResult)result;

		Assert.AreEqual((int)StatusCodes.BusinessException,okResult.StatusCode);
		Assert.AreEqual(_message,okResult.Value);
	}

	[TestCase]
	public void TestType_GetTestTypeAll_RepositoryException()
	{
		_testTypes.Append(_testType);
		_message = "GetById RepositoryException";
		_testTypeServiceMock.Setup(x => x.GetAll()).Throws(new RepositoryException(_message));
		IActionResult result = _testTypeController.GetTestTypeAll();
		Assert.IsInstanceOf<ObjectResult>(result);

		var okResult = (ObjectResult)result;

		Assert.AreEqual((int)StatusCodes.RepositoryException,okResult.StatusCode);
		Assert.AreEqual(_message,okResult.Value);
	}

	[TestCase]
	public void TestType_GetTestTypeAll_Exception()
	{
		_testTypes.Append(_testType);
		_message = "GetById Exception";
		_testTypeServiceMock.Setup(x => x.GetAll()).Throws(new Exception(_message));
		IActionResult result = _testTypeController.GetTestTypeAll();
		Assert.IsInstanceOf<ObjectResult>(result);

		var okResult = (ObjectResult)result;

		Assert.AreEqual((int)StatusCodes.ControllerException,okResult.StatusCode);
		Assert.AreEqual(_message,okResult.Value);
		Utilities.LoggerUtility.VerifyException(_message,_loggerMock);
	}

}