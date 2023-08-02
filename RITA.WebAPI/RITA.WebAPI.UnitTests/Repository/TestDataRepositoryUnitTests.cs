using Moq;
using NUnit.Framework;
using RITA.EF;
using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Repository;
using RITA.WebAPI.UnitTests.Models;
using RockLib.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;
using TestDataModel = RITA.WebAPI.Repository.Models.TestDataModel;

namespace RITA.WebAPI.UnitTests.Repository
{
    [ExcludeFromCodeCoverage]
    public class TestDataRepositoryUnitTests
    {
        private Mock<ILogger> _loggerMock;
        private Mock<ITestDataModelValidator> _validatorMock;
        private Mock<RitaContext> _contextMock;
        private ITestDataRepository _repository;

        private string _message = string.Empty;

        public TestDataRepositoryUnitTests()
        {
            BuildData();
        }

        [SetUp]
        public void TestSetup()
        {
            _contextMock = new Mock<RitaContext>();
            _validatorMock = new Mock<ITestDataModelValidator>();
            _loggerMock = new Mock<ILogger>();
            _repository = new TestDataRepository(_contextMock.Object, _validatorMock.Object, _loggerMock.Object);
        }

        [TestCase]
        public void GetById_Success()
        {
            var testDataModel = new TestDataModel()
            {
                Id = 201,
                Name = "TestData1",
                IsSuspended = true,
                SuspendedBy = "Maidul",
                SuspendedOn = new DateTime(2023, 2, 22),
                CreatedBy = "Me",
                CreatedOn = new DateTime(2023, 2, 22),
                TestCaseId = 102,
                TestTypeId = 601,
                Comment = string.Empty,
                StatusCode = HttpStatusCode.OK,
                RequestContent = "{\"LoanIdentifier\": \"9999999999\" }",
                RequestContentTypeId = 501,
                ResponseContent = "{\"LoanIdentifier\": \"9999999999\" }",
                ResponseContentTypeId = 501
            };

            var testDataRepository = new TestDataRepository(Context, _validatorMock.Object, _loggerMock.Object);
            ITestDataModel testDataList = testDataRepository.GetById(201);

            var propertyInfo = testDataModel.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                Assert.AreEqual(property.GetValue(testDataModel), property.GetValue(testDataList), property.Name);
            }
        }

        [TestCase]
        public void GetById_Null()
        {
            var testDataRepository = new TestDataRepository(Context, _validatorMock.Object, _loggerMock.Object);
            ITestDataModel testDataList = testDataRepository.GetById(2001);

            Assert.IsNull(testDataList);
        }

        [TestCase]
        public void GetById_Exception()
        {
            _message = "GetById Exception";
            _contextMock
                .Setup(d => d.TestData.Find(It.IsAny<int>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.GetById(1));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }


        [TestCase]
        public void Insert_Success()
        {
            var testDataModel = new TestDataModel();

            _contextMock
                .Setup(d => d.TestData.Add(It.IsAny<TestData>()))
                .Callback((TestData t) => t.Id = 211);
            _contextMock
                .Setup(d => d.SaveChanges())
                .Verifiable();

            ITestDataModel returnValue = _repository.Insert(testDataModel);
            Assert.AreEqual(211, returnValue.Id);
        }

        [TestCase]
        public void Insert_Exception()
        {
            var testDataModel = new TestDataModel();
            _message = "Insert Exception";
            _contextMock
                .Setup(d => d.TestData.Add(It.IsAny<TestData>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.Insert(testDataModel));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Insert_ValidationException()
        {
            var testDataModel = new TestDataModel();
            var field = new InvalidField() { Message = "Error", Name = "FieldName" };
            var validationException = new ValidationException("TestData Data contains invalid fields", new[] { field });
            _validatorMock
                .Setup(v => v.ValidateInsert(It.IsAny<TestDataModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _repository.Insert(testDataModel));
        }

        [TestCase]
        public void Update_Success()
        {
            var testDataModel = new TestDataModel() { Id = 2012 };
            _contextMock.Setup(d => d.TestData.Update(It.IsAny<TestData>()))
                .Callback((TestData s) => s.Id = 2012);
            _contextMock.Setup(d => d.SaveChanges())
                .Verifiable();

            ITestDataModel returnValue = _repository.Update(testDataModel);
            Assert.AreEqual(2012, returnValue.Id);
        }

        [TestCase]
        public void Update_Exception()
        {
            var testDataModel = new TestDataModel();
            _message = "Update Exception";
            _contextMock
                .Setup(d => d.TestData.Update(It.IsAny<TestData>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.Update(testDataModel));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }

        [TestCase]
        public void Update_ValidationException()
        {
            var testDataModel = new TestDataModel();
            var field = new InvalidField() { Message = "Error", Name = "FieldName" };
            var validationException = new ValidationException("TestData Data contains invalid fields", new[] { field });
            _validatorMock
                .Setup(v => v.ValidateUpdate(It.IsAny<TestDataModel>()))
                .Throws(validationException);

            Assert.Throws<ValidationException>(() => _repository.Update(testDataModel));
        }

        [TestCase]
        public void Delete_Success()
        {
            var testData = new TestData()
            {
                Id = 2001,
                CreatedOn = DateTime.Now
            };

            _contextMock
                .Setup(c => c.TestData.Find(It.IsAny<int>()))
                .Returns(testData);
            _contextMock.Setup(d => d.TestData.Remove(It.IsAny<TestData>()))
                .Callback((TestData s) => s.Id = 3);
            _contextMock
                .Setup(d => d.SaveChanges())
                .Verifiable();

            var returnValue = _repository.Delete(2001);
            Assert.AreEqual(2001, returnValue);
        }

        [TestCase]
        public void Delete_SuiteNotFound()
        {
            _contextMock
                .Setup(c => c.TestData.Find(It.IsAny<int>()))
                .Returns((TestData)null);

            var returnValue = _repository.Delete(2002);
            Assert.AreEqual(2002, returnValue);
        }

        [TestCase]
        public void Delete_Exception()
        {
            _message = "Delete Exception";
            var testData = new TestData()
            {
                Id = 2013,
                CreatedOn = DateTime.Now
            };

            _contextMock
                .Setup(c => c.TestData.Find(It.IsAny<int>()))
                .Returns(testData);
            _contextMock.Setup(d => d.TestData.Remove(It.IsAny<TestData>()))
                .Throws(new Exception(_message));

            Assert.Throws<RepositoryException>(() => _repository.Delete(2013));
            Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
        }
    }
}