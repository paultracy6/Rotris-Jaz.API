using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;

namespace RITA.WebAPI.Business.Services
{
    public class TestTypeService : ITestTypeService
    {
        private readonly ITestTypeRepository _testTypeRepository;
        private readonly ILogger _logger;

        public TestTypeService(ITestTypeRepository testTypeRepository, ILogger logger)
        {
            _testTypeRepository = testTypeRepository;
            _logger = logger;
        }

        public IEnumerable<ITestTypeView> GetAll()
        {
            try
            {
                return _testTypeRepository.GetAll() as dynamic;
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
