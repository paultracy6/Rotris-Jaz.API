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
    public class TestCaseService : ITestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ITestCaseValidator _testCaseValidator;
        private readonly ILogger _logger;

        public TestCaseService(ITestCaseRepository testCaseRepository, ITestCaseValidator testCaseValidator, ILogger logger)
        {
            _testCaseRepository = testCaseRepository;
            _testCaseValidator = testCaseValidator;
            _logger = logger;
        }

        public ITestCaseView? GetById(int id)
        {
            ITestCaseModel? response = _testCaseRepository.GetById(id);

            return response?.ToView();
        }


        public ITestCaseView Insert(ITestCaseView view, string createdBy)
        {
            try
            {
                ITestCaseModel testCase = view.ToModel();
                testCase.CreatedBy = createdBy;
                testCase.CreatedOn = DateTime.UtcNow;

                _testCaseValidator.ValidateInsert(testCase);
                var response = _testCaseRepository.Insert(testCase);
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

        public ITestCaseView Update(ITestCaseView testCaseView, string updatedBy)
        {
            try
            {
                ITestCaseModel testCase = testCaseView.ToModel();
                testCase.UpdatedBy = updatedBy;
                testCase.UpdatedOn = DateTime.UtcNow;

                _testCaseValidator.ValidateUpdate(testCase);
                return _testCaseRepository.Update(testCase).ToView();
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
                return _testCaseRepository.Delete(id);
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

        public IEnumerable<ITestDataView> GetTestData(int testCaseId)
        {
            try
            {
                return _testCaseRepository.GetTestData(testCaseId).ToViews();
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
