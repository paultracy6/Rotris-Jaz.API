using Moq;
using NUnit.Framework;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Models;
using RITA.WebAPI.Business.Services;
using RITA.WebAPI.Business.Views;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.UnitTests.Utilities;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;
using ILogger = RockLib.Logging.ILogger;
using InvalidField = RITA.WebAPI.UnitTests.Models.InvalidField;
using SuiteModel = RITA.WebAPI.UnitTests.Models.SuiteModel;
using SuiteView = RITA.WebAPI.UnitTests.Views.SuiteView;

namespace RITA.WebAPI.UnitTests.Business
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SuiteServiceUnitTests
    {
        private readonly Mock<ISuiteRepository> _suiteRepositoryMock;
        private readonly Mock<ISuiteValidator> _validatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly ISuiteService _suiteService;
        private ISuiteView _suiteView = new SuiteView();
        private ISuiteModel _suiteModel = new SuiteModel();
        private string _message = string.Empty;

        public SuiteServiceUnitTests()
        {
            _suiteRepositoryMock = new Mock<ISuiteRepository>();
            _validatorMock = new Mock<ISuiteValidator>();
            _loggerMock = new Mock<ILogger>();
            _suiteService = new SuiteService(_suiteRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [SetUp]
        public void TestSetup()
        {
            _suiteRepositoryMock.Reset();
            _validatorMock.Reset();
            _loggerMock.Reset();
            _suiteView = new SuiteView();
            _suiteModel = new SuiteModel();
        }

        [TestCase]
        public void GetById_ReturnData()
        {
            _suiteModel.Id = 2;
            _suiteModel.AppId = 1;
            _suiteModel.Name = "Jazz";
            _suiteRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(_suiteModel);

            var actual = _suiteService.GetById(3);

            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual?.Id);
        }

        [TestCase]
        public void GetById_DataNotFound()
        {
            _suiteRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns<ISuiteModel>(null);

            Assert.Null(_suiteService.GetById(1));
        }

        [TestCase]
        public void GetById_RepositoryException()
        {
            _suiteRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Throws(new RepositoryException("GetById Exception"));

            Assert.Throws<RepositoryException>(() => _suiteService.GetById(1));
        }

        [TestCase]
        public void Insert_FailedValidation()
        {
            var field = new InvalidField() { Message = "Error", Name = "FieldName" };
            var validationException = new ValidationException("Suite Data contains invalid fields", new List<IInvalidField>() { field });
            _validatorMock.Setup(x => x.ValidateInsert(It.IsAny<ISuiteModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _suiteService.Insert(_suiteView, "Jazz"));
        }

        [TestCase]
        public void Insert_Exception()
        {
            _validatorMock.Setup(x => x.ValidateInsert(It.IsAny<ISuiteModel>()))
                .Throws(new Exception("Test Exception"));

            Assert.Throws<BusinessException>(() => _suiteService.Insert(_suiteView, "Jazz"));

            _loggerMock.Verify(x =>
                x.Log(It.Is<LogEntry>(le => le.Level == LogLevel.Error && le.Message != null && le.Message.Contains("Test Exception")),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestCase]
        public void Insert_RepositoryException()
        {
            _suiteRepositoryMock.Setup(x => x.Insert(It.IsAny<ISuiteModel>()))
                .Throws(new RepositoryException("Test Repository Exception"));

            Assert.Throws<RepositoryException>(() => _suiteService.Insert(_suiteView, "Jazz"));

        }

        [TestCase]
        public void Insert_Success()
        {
            _suiteModel.Id = 2;
            _suiteView.Id = 0;
            _suiteRepositoryMock.Setup(x => x.Insert(It.IsAny<ISuiteModel>()))
                .Returns(_suiteModel);
            var results = _suiteService.Insert(_suiteView, "Jazz");

            Assert.AreEqual(2, results.Id);
        }

        [TestCase]
        public void Update_FailedValidation()
        {
            var field = new InvalidField() { Message = "Error", Name = "FieldName" };
            var validationException = new ValidationException("Suite Data contains invalid fields", new IInvalidField[] { field });
            _validatorMock.Setup(x => x.ValidateUpdate(It.IsAny<ISuiteModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _suiteService.Update(_suiteView, "Jazz"));
        }

        [TestCase]
        public void Update_Exception()
        {
            _validatorMock.Setup(x => x.ValidateUpdate(It.IsAny<ISuiteModel>()))
                .Throws(new Exception("Test Exception"));

            Assert.Throws<BusinessException>(() => _suiteService.Update(_suiteView, "Jazz"));

            _loggerMock.Verify(x =>
                x.Log(It.Is<LogEntry>(le => le.Level == LogLevel.Error && le.Message != null && le.Message.Contains("Test Exception")),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestCase]
        public void Update_RepositoryException()
        {
            _suiteRepositoryMock.Setup(x => x.Update(It.IsAny<ISuiteModel>()))
                .Throws(new RepositoryException("Test Repository Exception"));

            Assert.Throws<RepositoryException>(() => _suiteService.Update(_suiteView, "Jazz"));
        }

        [TestCase]
        public void Update_Success()
        {
            _suiteModel.Id = 1;
            _suiteView.Id = 3;
            _suiteRepositoryMock.Setup(x => x.Update(It.IsAny<ISuiteModel>()))
                .Returns(_suiteModel);
            var results = _suiteService.Update(_suiteView, "Jazz");

            Assert.AreEqual(1, results.Id);
        }

        [TestCase]
        public void Delete_Exception()
        {
            _suiteRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new BusinessException("Test Exception"));

            Assert.Throws<BusinessException>(() => _suiteService.Delete(1));

            _loggerMock.Verify(x =>
                x.Log(It.Is<LogEntry>(le => le.Level == LogLevel.Error && le.Message != null && le.Message.Contains("Test Exception")),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestCase]
        public void Delete_RepositoryException()
        {
            _suiteRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new RepositoryException("Test Repository Exception"));

            Assert.Throws<RepositoryException>(() => _suiteService.Delete(1));
        }

        [TestCase]
        public void Delete_Success()
        {
            _suiteRepositoryMock.Setup(x => x.Delete(It.IsAny<int>()))
                .Returns(1);
            var results = _suiteService.Delete(1);

            Assert.AreEqual(1, results);
        }

        [TestCase]
        public void GetTestCasesBySuiteId_Success()
        {
            List<ITestCaseView> response = new List<ITestCaseView>();
            response.Add(new TestCaseView());
            var model = new List<ITestCaseModel>() { new TestCaseModel() };
            _suiteRepositoryMock.Setup(x => x.GetTestCases(It.IsAny<int>()))
                .Returns(model);

            ComparerUtility.AreEqual(response, _suiteService.GetTestCasesBySuiteId(5));
        }

        [TestCase]
        public void GetTestCasesBySuiteId_RepositoryException()
        {
            _message = "GetTestCasesBySuiteId Repo Exception";
            _suiteRepositoryMock.Setup(x => x.GetTestCases(It.IsAny<int>()))
                .Throws(new RepositoryException(_message));

            Assert.Throws<RepositoryException>(() => _suiteService.GetTestCasesBySuiteId(5));

        }

        [TestCase]
        public void GetTestCasesBySuiteId_Exception()
        {
            _message = "GetTestCasesBySuiteId Exception";
            _suiteRepositoryMock.Setup(x => x.GetTestCases(It.IsAny<int>()))
                .Throws(new Exception(_message));

            Assert.Throws<BusinessException>(() => _suiteService.GetTestCasesBySuiteId(5));

            LoggerUtility.VerifyException(_message, _loggerMock);
        }
    }
}