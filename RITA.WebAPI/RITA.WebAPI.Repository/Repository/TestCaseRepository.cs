using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Converters;
using RockLib.Logging;

namespace RITA.WebAPI.Repository.Repository;

public class TestCaseRepository : ITestCaseRepository
{
    private readonly ILogger _logger;
    private readonly RitaContext _context;
    private readonly ITestCaseModelValidator _validator;

    public TestCaseRepository(RitaContext context, ITestCaseModelValidator validator, ILogger logger)
    {
        _logger = logger;
        _context = context;
        _validator = validator;
    }

    public ITestCaseModel GetById(int id)
    {
        try
        {
            var testCase = _context.TestCases.Find(id);

            return testCase?.ToITestCaseModel();
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }

    public ITestCaseModel Insert(ITestCaseModel entity)
    {
        try
        {
            _validator.ValidateInsert(entity);

            var testCase = entity.ToTestCase();
            _context.TestCases.Add(testCase);
            _context.SaveChanges();

            return testCase.ToITestCaseModel();
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

    public ITestCaseModel Update(ITestCaseModel entity)
    {
        try
        {
            _validator.ValidateUpdate(entity);

            var testCase = entity.ToTestCase();
            _context.TestCases.Update(testCase);
            _context.SaveChanges();

            return testCase.ToITestCaseModel();
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
            var testCase = _context.TestCases.Find(id);
            if (testCase == null) return id;
            _context.TestCases.Remove(testCase);
            _context.SaveChanges();

            return id;
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }


    public IEnumerable<ITestDataModel> GetTestData(int testCaseId)
    {
        try
        {
            var testData = _context.TestData.Where(t => t.TestCaseId == testCaseId);
            return testData.ToITestDataModelEnumerable();
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }
}