using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Api.Models;
using RITA.WebAPI.Business.Services;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.UnitTests.Models;
using RITA.WebAPI.UnitTests.Utilities;
using RITA.WebAPI.UnitTests.Views;
using System.Diagnostics.CodeAnalysis;
using ILogger = RockLib.Logging.ILogger;

namespace RITA.WebAPI.UnitTests.Business
{
    [ExcludeFromCodeCoverage]
    public class TestDataServiceUnitTests
    {
        private Mock<ITestDataRepository> _testDataRepositoryMock;
        private Mock<ITestDataValidator> _validatorMock;
        private Mock<ILogger> _loggerMock;
        private ITestDataService _testDataService;
        private ITestDataModel _testDataModel;
        private ITestDataView _testDataView;
        private string _message = string.Empty;

        public TestDataServiceUnitTests()
        {

        }

        [SetUp]
        public void TestSetup()
        {
            _testDataRepositoryMock = new Mock<ITestDataRepository>();
            _validatorMock = new Mock<ITestDataValidator>();
            _loggerMock = new Mock<ILogger>();
            _testDataService =
                new TestDataService(_testDataRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object);
            _testDataModel = new TestDataModel();
            _testDataView = new TestDataView();
        }

        [TestCase]
        public void GetById_Success()
        {
            _testDataModel = new TestDataModel();
            _testDataRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(_testDataModel);

            Assert.AreEqual(_testDataModel, _testDataService.GetById(5));
        }

        [TestCase]
        public void GetById_NotFound()
        {
            _testDataModel = null;
            _testDataRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(_testDataModel);

            Assert.Null(_testDataService.GetById(5));
        }

        [TestCase]
        public void Insert_Success()
        {
            _testDataModel.Id = 0;
            _testDataRepositoryMock.Setup(x => x.Insert(It.IsAny<ITestDataModel>()))
                .Returns(_testDataModel);
            var results = _testDataService.Insert(_testDataView, "API");

            Assert.AreEqual(_testDataModel, results);
        }

        [TestCase]
        public void Insert_ValidationException()
        {
            var validationException = new ValidationException("Badata", Array.Empty<IInvalidField>());
            _validatorMock.Setup(x => x.ValidateInsert(It.IsAny<ITestDataModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _testDataService.Insert(_testDataView, "API"));

        }

        [TestCase]
        public void Insert_RepositoryException()
        {
            _message = "Test Insert Repository Exception";
            _testDataRepositoryMock.Setup(x => x.Insert(It.IsAny<ITestDataModel>()))
                .Throws(new RepositoryException(_message));

            Assert.Throws<RepositoryException>(() => _testDataService.Insert(_testDataView, "API"));
        }

        [TestCase]
        public void Insert_Exception()
        {
            _message = "Test Insert Exception";
            _validatorMock.Setup(x => x.ValidateInsert(It.IsAny<ITestDataModel>()))
                .Throws(new Exception(_message));

            Assert.Throws<BusinessException>(() => _testDataService.Insert(_testDataView, "API"));

            LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Update_Success()
        {
            _testDataModel.Id = 7;
            _testDataRepositoryMock.Setup(x => x.Update(It.IsAny<ITestDataModel>()))
                .Returns(_testDataModel);
            var results = _testDataService.Update(_testDataView, "API");

            Assert.AreEqual(_testDataModel, results);
        }

        [TestCase]
        public void Update_ValidationException()
        {
            var validationException = new ValidationException("Baddata", Array.Empty<IInvalidField>());
            _validatorMock.Setup(x => x.ValidateUpdate(It.IsAny<ITestDataModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _testDataService.Update(_testDataView, "API"));
        }

        [TestCase]
        public void Update_RepositoryException()
        {
            _message = "Test Update Repository Exception";
            _testDataRepositoryMock.Setup(x => x.Update(It.IsAny<ITestDataModel>()))
                .Throws(new RepositoryException(_message));

            Assert.Throws<RepositoryException>(() => _testDataService.Update(_testDataView, "API"));
        }

        [TestCase]
        public void Update_Exception()
        {
            _message = "Test Update Exception";
            _validatorMock.Setup(x => x.ValidateUpdate(It.IsAny<ITestDataModel>()))
                .Throws(new Exception(_message));

            Assert.Throws<BusinessException>(() => _testDataService.Update(_testDataView, "API"));

            LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Delete_Success()
        {
            _testDataRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(5);
            var results = _testDataService.Delete(5);

            Assert.AreEqual(5, results);
        }

        [TestCase]
        public void Delete_RepositoryException()
        {
            _message = "Test Delete Repository Exception";
            _testDataRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new RepositoryException(_message));

            Assert.Throws<RepositoryException>(() => _testDataService.Delete(3));
        }

        [TestCase]
        public void Delete_Exception()
        {
            _message = "Test Delete Exception";
            _testDataRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new Exception(_message));

            Assert.Throws<BusinessException>(() => _testDataService.Delete(9));

            LoggerUtility.VerifyException(_message, _loggerMock);
        }
    }
}
