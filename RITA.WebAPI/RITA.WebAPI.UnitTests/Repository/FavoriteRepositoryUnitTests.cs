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
using static RITA.WebAPI.UnitTests.Utilities.RitaDbContext;

namespace RITA.WebAPI.UnitTests.Repository;

[ExcludeFromCodeCoverage]
[TestFixture]
public class FavoriteRepositoryUnitTests
{
    private Mock<ILogger> _loggerMock;
    private Mock<IFavoriteModelValidator> _validatorMock;
    private Mock<RitaContext> _contextMock;
    private IFavoriteRepository _repository;

    private string _message = string.Empty;

    public FavoriteRepositoryUnitTests()
    {
        BuildData();
        _loggerMock = new Mock<ILogger>();
        _validatorMock = new Mock<IFavoriteModelValidator>();
    }

    [SetUp]
    public void TestSetup()
    {
        _contextMock = new Mock<RitaContext>();
        _validatorMock.Reset();
        _loggerMock.Reset();
        _repository = new FavoriteRepository(_contextMock.Object, _validatorMock.Object, _loggerMock.Object);
    }

    [TestCase]

    public void GetFavoritesByUserId_Success()
    {
        var userId = "user1";

        var actual = _repository.GetFavoritesByUserId(userId);

        Assert.AreEqual(2, actual.Count());
    }

    [TestCase]

    public void GetFavoritesByUserId_Exception()
    {
        _message = "GetFavoritesByUserId Exception";
        _contextMock
            .Setup(f => f.Favorites)
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.GetFavoritesByUserId("user1"));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Delete_Success()
    {
        var favorite = new Favorite()
        {
            Id = 1234
        };

        _contextMock
            .Setup(f => f.Favorites.Find(It.IsAny<int>()))
            .Returns(favorite);
        _contextMock
            .Setup(f => f.Favorites.Remove(It.IsAny<Favorite>()))
            .Callback((Favorite f) => f.Id = 1234);
        _contextMock
            .Setup(f => f.SaveChanges())
            .Verifiable();

        var returnValue = _repository.Delete(1234);
        Assert.AreEqual(1234, returnValue);
    }

    [TestCase]
    public void Delete_FavoriteNotFound()
    {
        _contextMock
            .Setup(f => f.Favorites.Find(It.IsAny<int>()))
            .Returns((Favorite)null);

        var returnValue = _repository.Delete(1234);
        Assert.AreEqual(1234, returnValue);
    }

    [TestCase]
    public void Delete_Exception()
    {
        _message = "Delete Exception";
        var favorite = new Favorite()
        {
            Id = 1234
        };

        _contextMock
            .Setup(f => f.Favorites.Find(It.IsAny<int>()))
            .Returns(favorite);
        _contextMock.Setup(f => f.Favorites.Remove(It.IsAny<Favorite>()))
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.Delete(234));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Insert_Success()
    {
        var favoriteModel = new FavoriteModel();

        _contextMock
            .Setup(d => d.Favorites.Add(It.IsAny<Favorite>()))
            .Callback((Favorite f) => f.Id = 10);
        _contextMock
            .Setup(d => d.SaveChanges())
            .Verifiable();

        IFavoriteModel returnValue = _repository.Insert(favoriteModel);
        Assert.AreEqual(10, returnValue.Id);
    }

    [TestCase]
    public void Insert_Exception()
    {
        var favoriteModel = new FavoriteModel();
        _message = "Insert Exception";
        _contextMock
            .Setup(d => d.Favorites.Add(It.IsAny<Favorite>()))
            .Throws(new Exception(_message));

        Assert.Throws<RepositoryException>(() => _repository.Insert(favoriteModel));
        Utilities.LoggerUtility.VerifyException(_message, _loggerMock);
    }

    [TestCase]
    public void Insert_ValidationException()
    {
        var favoriteModel = new FavoriteModel();
        var field = new InvalidField() { Message = "Error", Name = "FieldName" };
        var validationException = new ValidationException("Favorite Data contains invalid fields", new[] { field });
        _validatorMock
            .Setup(v => v.ValidateInsert(It.IsAny<IFavoriteModel>()))
            .Throws(validationException);

        Assert.Throws<ValidationException>(() => _repository.Insert(favoriteModel));
    }

}
