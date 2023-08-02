using Moq;
using NUnit.Framework;
using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Repository;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;
using Assert = NUnit.Framework.Assert;
using InvalidField = RITA.WebAPI.Api.Models.InvalidField;
using Suite = RITA.EF.Models.Suite;
using SuiteModel = RITA.WebAPI.Repository.Models.SuiteModel;

namespace RITA.WebAPI.UnitTests.Repository
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
    public class SuiteRepositoryUnitTests
    {
        private Mock<ILogger> _loggerMock;
        private Mock<ISuiteModelValidator> _validatorMock;
        private Mock<RitaContext> _contextMock;
        private ISuiteRepository _repository;

        private string _message = string.Empty;

        public SuiteRepositoryUnitTests()
        {
            BuildData();
        }

        [SetUp]
        public void TestSetup()
        {
            _contextMock = new Mock<RitaContext>();
            _validatorMock = new Mock<ISuiteModelValidator>();
            _loggerMock = new Mock<ILogger>();
            _repository = new SuiteRepository(_contextMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [TestCase]
        public void GetById_Success()
        {
            var dateTime = DateTime.UtcNow;
            var suite = new Suite()
            {
                Id = 1,
                CreatedOn = dateTime
            };
            var suiteModel = new SuiteModel()
            {
                Id = 1,
                CreatedOn = dateTime
            };
            _contextMock
                .Setup(c => c.Suites.Find(It.IsAny<int>()))
                .Returns(suite);

            var actual = _repository.GetById(1);

            PropertyInfo[] propertyInfo = suiteModel.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                Assert.AreEqual(property.GetValue(suiteModel), property.GetValue(actual));
            }
        }

        [TestCase]
        public void GetById_Exception()
        {
            _message = "GetById Exception";
            _contextMock
                .Setup(d => d.Suites.Find(It.IsAny<int>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.GetById(1));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Insert_Success()
        {
            var suiteModel = new SuiteModel();

            _contextMock
                .Setup(d => d.Suites.Add(It.IsAny<Suite>()))
                .Callback((Suite s) => s.Id = 1);
            _contextMock
                .Setup(d => d.SaveChanges())
                .Verifiable();

            ISuiteModel returnValue = _repository.Insert(suiteModel);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestCase]
        public void Insert_Exception()
        {
            var suiteModel = new SuiteModel();
            _message = "Insert Exception";
            _contextMock
                .Setup(d => d.Suites.Add(It.IsAny<Suite>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.Insert(suiteModel));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Insert_ValidationException()
        {
            var suiteModel = new SuiteModel();
            var field = new InvalidField() { Message = "Error", Name = "FieldName" };
            var validationException = new ValidationException("Suite Data contains invalid fields", new[] { field });
            _validatorMock
                .Setup(v => v.ValidateInsert(It.IsAny<SuiteModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _repository.Insert(suiteModel));
        }

        [TestCase]
        public void Update_Success()
        {
            var suiteModel = new SuiteModel() { Id = 2 };

            _contextMock.Setup(d => d.Suites.Update(It.IsAny<Suite>()))
                .Callback((Suite s) => s.Id = 2);
            _contextMock.Setup(d => d.SaveChanges())
                .Verifiable();

            ISuiteModel returnValue = _repository.Update(suiteModel);
            Assert.AreEqual(2, returnValue.Id);
        }

        [TestCase]
        public void Update_Exception()
        {
            var suiteModel = new SuiteModel();
            _message = "Update Exception";
            _contextMock
                .Setup(d => d.Suites.Update(It.IsAny<Suite>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.Update(suiteModel));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Update_ValidationException()
        {
            var suiteModel = new SuiteModel();
            var field = new InvalidField() { Message = "Error", Name = "FieldName" };
            var validationException = new ValidationException("Suite Data contains invalid fields", new[] { field });
            _validatorMock
                .Setup(v => v.ValidateUpdate(It.IsAny<SuiteModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _repository.Update(suiteModel));
        }

        [TestCase]
        public void Delete_Success()
        {
            var suite = new Suite()
            {
                Id = 3,
                CreatedOn = DateTime.Now
            };

            _contextMock
                .Setup(c => c.Suites.Find(It.IsAny<int>()))
                .Returns(suite);
            _contextMock.Setup(d => d.Suites.Remove(It.IsAny<Suite>()))
                .Callback((Suite s) => s.Id = 3);
            _contextMock
                .Setup(d => d.SaveChanges())
                .Verifiable();

            var returnValue = _repository.Delete(3);
            Assert.AreEqual(3, returnValue);
        }

        [TestCase]
        public void Delete_SuiteNotFound()
        {
            _contextMock
                .Setup(c => c.Suites.Find(It.IsAny<int>()))
                .Returns((Suite)null);

            var returnValue = _repository.Delete(4);
            Assert.AreEqual(4, returnValue);
        }

        [TestCase]
        public void Delete_Exception()
        {
            _message = "Delete Exception";
            var suite = new Suite()
            {
                Id = 3,
                CreatedOn = DateTime.Now
            };

            _contextMock
                .Setup(c => c.Suites.Find(It.IsAny<int>()))
                .Returns(suite);
            _contextMock.Setup(d => d.Suites.Remove(It.IsAny<Suite>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.Delete(3));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void GetTestCasesBySuiteId_Success()
        {
            SuiteRepository suites = new SuiteRepository(Context, _validatorMock.Object, _loggerMock.Object);
            IEnumerable<ITestCaseModel> testCaseList = suites.GetTestCases(1);

            Assert.AreEqual(2, testCaseList.Count());
        }

        [TestCase]
        public void GetTestCasesBySuiteId_Success_1of3()
        {
            var suites = new SuiteRepository(Context, _validatorMock.Object, _loggerMock.Object);
            IEnumerable<ITestCaseModel> testCaseList = suites.GetTestCases(3);

            Assert.AreEqual(1, testCaseList.Count());
        }

        [TestCase]
        public void GetTestCasesBySuiteId_Exception()
        {
            _message = "GetTestCasesBySuiteId Exception";
            _contextMock.Setup(d => d.TestCases)
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.GetTestCases(10));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }
    }
}