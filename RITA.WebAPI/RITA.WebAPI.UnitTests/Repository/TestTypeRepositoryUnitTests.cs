using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RITA.EF;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Converters;
using RITA.WebAPI.Repository.Models;
using RITA.WebAPI.Repository.Repository;
using RockLib.Logging;
using System.Reflection;
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;
using TestType = RITA.EF.Models.TestType;

namespace RITA.WebAPI.UnitTests.Repository;

public class TestTypeRepositoryUnitTests
{
    private Mock<ILogger> _loggerMock;
    private Mock<RitaContext> _contextMock;
    private ITestTypeRepository _repository;

    public TestTypeRepositoryUnitTests()
    {
        BuildData();
    }

    [SetUp]
    public void TestSetUp()
    {
        _contextMock = new Mock<RitaContext>();
        _loggerMock = new Mock<ILogger>();
        _repository = new TestTypeRepository(_contextMock.Object, _loggerMock.Object);
    }

    [TestCase]
    public void GetAllTestTypes_Success()
    {
        var testTypeModel = new TestTypeModel()
        {
            Id = 601,
            Name = "Integration",
            CreatedBy = "Me",
            CreatedOn = new DateTime(2023, 2, 22),
            UpdatedOn = new DateTime(2023, 2, 24),
            UpdatedBy = "Me2"
        };

        var testTypeRepository = new TestTypeRepository(Context, _loggerMock.Object);
        var actual = testTypeRepository.GetAll().ToList().First();

        var propertyInfo = testTypeModel.GetType().GetProperties();
        foreach (PropertyInfo property in propertyInfo)
        {
            Assert.AreEqual(property.GetValue(testTypeModel), property.GetValue(actual), property.Name);
        }
    }

    [TestCase]
    public void GetAllTestTypes_Null()
    {
        _contextMock
            .Setup(d => d.TestTypes)
            .Returns((DbSet<TestType>)null);

        var testTypeList = _repository.GetAll();

        Assert.IsNull(testTypeList);
    }

    [TestCase]
    public void GetAllTestTypes_Exception()
    {
        var message = "GetTestData Exception";
        _contextMock
            .Setup(d => d.TestTypes)
            .Throws(new Exception(message));

        Assert.Throws<RepositoryException>(() => _repository.GetAll());
        Utilities.LoggerUtility.VerifyException(message, _loggerMock);
    }

    [TestCase]
    public void Converter_ToITestTypeModel_ListNull()
    {
        var testTypeList = TestTypeConverter.ToITestTypeModelEnumerable(null);
        Assert.AreEqual(0, testTypeList.Count());
    }
}