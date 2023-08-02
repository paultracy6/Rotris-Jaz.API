using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Converters;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;

namespace RITA.WebAPI.Business.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ILogger _logger;

    public ApplicationService(IApplicationRepository applicationRepository, ILogger logger)
    {
        _applicationRepository = applicationRepository;
        _logger = logger;
    }

    public IEnumerable<ISuiteView> GetSuiteByAppId(int appId)
    {
        try
        {
            return _applicationRepository.GetSuitesByAppId(appId).ToViews();
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

    public IEnumerable<ITestCaseView> GetTestCaseByAppId(int appId)
    {
        try
        {
            return _applicationRepository.GetTestCasesByAppId(appId).ToViews();
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