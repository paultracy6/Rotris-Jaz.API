using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;

namespace RITA.WebAPI.Business.Services
{
    public class TestDataService : ITestDataService
    {
        private readonly ITestDataRepository _testDataRepository;
        private readonly ITestDataValidator _testDataValidator;
        private readonly ILogger _logger;

        public TestDataService(ITestDataRepository testDataRepository, ITestDataValidator testDataValidator, ILogger logger)
        {
            _testDataRepository = testDataRepository;
            _testDataValidator = testDataValidator;
            _logger = logger;
        }

        public ITestDataView? GetById(int id)
        {
            return _testDataRepository.GetById(id) as dynamic;
        }

        public ITestDataView Insert(ITestDataView view, string createdBy)
        {
            throw new NotImplementedException();
        }

        public ITestDataView Update(ITestDataView view, string updatedBy)
        {
            throw new NotImplementedException();
        }

        public ITestDataView Insert(ITestDataView view)
        {
            throw new NotImplementedException();
        }

        public ITestDataView Update(ITestDataView view)
        {
            throw new NotImplementedException();
        }

        public ITestDataModel Insert(ITestDataModel entity)
        {
            try
            {
                _testDataValidator.ValidateInsert(entity);
                return _testDataRepository.Insert(entity);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new BusinessException(e.Message, e);
            }

        }

        public ITestDataModel Update(ITestDataModel entity)
        {
            try
            {
                _testDataValidator.ValidateUpdate(entity);
                return _testDataRepository.Update(entity);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new BusinessException(e.Message, e);
            }
        }

        ITestDataView? IBaseService<ITestDataView>.GetById(int id)
        {
            return GetById(id);
        }

        public int Delete(int id)
        {
            try
            {
                return _testDataRepository.Delete(id);
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new BusinessException(e.Message, e);
            }
        }
    }
}
