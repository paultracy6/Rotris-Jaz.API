using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Converters;
using RockLib.Logging;

namespace RITA.WebAPI.Repository.Repository;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ILogger _logger;
    private readonly RitaContext _context;

    public ApplicationRepository(RitaContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<ISuiteModel> GetSuitesByAppId(int appId)
    {
        try
        {
            var suites = _context.Suites.Where(s => s.AppId == appId);
            return suites.ToISuiteModelEnumerable();
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }

    public IEnumerable<ITestCaseModel> GetTestCasesByAppId(int appId)
    {
        try
        {
            var testCases = _context.TestCases
                .Join(_context.Suites.Where(d => d.AppId == appId),
                    tc => tc.SuiteId,
                    s => s.Id,
                    (tc, s) => tc);

            return testCases.ToITestCaseModelEnumerable();
        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }
}