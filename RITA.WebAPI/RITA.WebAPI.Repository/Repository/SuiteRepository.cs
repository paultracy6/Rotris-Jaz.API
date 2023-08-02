#nullable enable
using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Converters;
using RockLib.Logging;

namespace RITA.WebAPI.Repository.Repository
{

    public class SuiteRepository : ISuiteRepository
    {
        private readonly ILogger _logger;
        private readonly RitaContext _context;
        private readonly ISuiteModelValidator _validator;

        public SuiteRepository(RitaContext context, ISuiteModelValidator validator, ILogger logger)
        {
            _context = context;
            _validator = validator;
            _logger = logger;
        }

        public ISuiteModel? GetById(int id)
        {
            try
            {
                var suite = _context.Suites.Find(id);

                return suite?.ToISuiteModel();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new RepositoryException(e.Message, e);
            }
        }

        public ISuiteModel Insert(ISuiteModel entity)
        {
            try
            {
                var suite = entity.ToSuite();

                _validator.ValidateInsert(entity);
                _context.Suites.Add(suite);
                _context.SaveChanges();

                return suite.ToISuiteModel();
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

        public ISuiteModel Update(ISuiteModel entity)
        {
            try
            {
                var suite = entity.ToSuite();
                _validator.ValidateUpdate(entity);
                _context.Suites.Update(suite);
                _context.SaveChanges();

                return suite.ToISuiteModel();
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
                var suite = _context.Suites.Find(id);
                if (suite == null) return id;
                _context.Suites.Remove(suite);
                _context.SaveChanges();

                return id;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new RepositoryException(e.Message, e);
            }
        }

        public IEnumerable<ITestCaseModel> GetTestCases(int suiteId)
        {
            try
            {
                var testCases = _context.TestCases.Where(t => t.SuiteId == suiteId);
                return testCases.ToITestCaseModelEnumerable();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new RepositoryException(e.Message, e);
            }
        }
    }
}