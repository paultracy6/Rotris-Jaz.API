using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Converters;
using RockLib.Logging;

namespace RITA.WebAPI.Repository.Repository;

public class TestDataRepository : ITestDataRepository
{
    private readonly RitaContext _context;
    private readonly ITestDataModelValidator _validator;
    private readonly ILogger _logger;

    public TestDataRepository(RitaContext context, ITestDataModelValidator validator, ILogger logger)
    {
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public ITestDataModel GetById(int id)
    {
        try
        {
            var testData = _context.TestData.Find(id);

            return testData?.ToITestDataModel();
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }

    public ITestDataModel Insert(ITestDataModel entity)
    {
        try
        {
            _validator.ValidateInsert(entity);

            var testData = entity.ToTestData();
            _context.TestData.Add(testData);
            _context.SaveChanges();

            return testData.ToITestDataModel();
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }

    public ITestDataModel Update(ITestDataModel entity)
    {
        try
        {
            _validator.ValidateUpdate(entity);

            var testData = entity.ToTestData();
            _context.TestData.Update(testData);
            _context.SaveChanges();

            return testData.ToITestDataModel();
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }

    public int Delete(int id)
    {
        try
        {
            var testData = _context.TestData.Find(id);
            if (testData == null) return id;
            _context.TestData.Remove(testData);
            _context.SaveChanges();

            return id;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }
}