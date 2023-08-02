using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Converters;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;

namespace RITA.WebAPI.Business.Services
{
    public class SuiteService : ISuiteService
    {
        private readonly ISuiteRepository _suiteRepository;
        private readonly ISuiteValidator _suiteValidator;
        private readonly ILogger _logger;


        public SuiteService(ISuiteRepository suiteRepository, ISuiteValidator suiteValidator, ILogger logger)
        {
            _suiteRepository = suiteRepository;
            _suiteValidator = suiteValidator;
            _logger = logger;
        }

        public ISuiteView? GetById(int id)
        {
            ISuiteModel? response = _suiteRepository.GetById(id);

            return response?.ToView();
        }

        public ISuiteView Insert(ISuiteView view)
        {
            throw new NotImplementedException();
        }

        public ISuiteView Update(ISuiteView view)
        {
            throw new NotImplementedException();
        }

        public ISuiteView Insert(ISuiteView view, string createdBy)
        {
            try
            {
                //create an IService entity
                ISuiteModel suite = view.ToModel();
                suite.CreatedBy = createdBy;
                suite.CreatedOn = DateTime.UtcNow;
                //populate with model properties (Set created on and created by date/time)
                //convert repo response to a view
                _suiteValidator.ValidateInsert(suite);
                var response = _suiteRepository.Insert(suite);
                return response.ToView();
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


        public ISuiteView Update(ISuiteView view, string createdBy)
        {
            try
            {
                ISuiteModel suite = view.ToModel();
                suite.UpdatedBy = createdBy;
                suite.UpdatedOn = DateTime.UtcNow;

                _suiteValidator.ValidateUpdate(suite);
                var response = _suiteRepository.Update(suite);
                return response.ToView();
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

        public int Delete(int id)
        {
            try
            {
                return _suiteRepository.Delete(id);
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

        public IEnumerable<ITestCaseView> GetTestCasesBySuiteId(int suiteId)
        {
            try
            {
                IEnumerable<ITestCaseModel> response = _suiteRepository.GetTestCases(suiteId);

                return response.Select(x => x.ToView());
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
