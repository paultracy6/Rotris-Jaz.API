using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Converters;
using RockLib.Logging;

namespace RITA.WebAPI.Repository.Repository;

public class TestTypeRepository : ITestTypeRepository
{
    private readonly RitaContext _context;
    private readonly ILogger _logger;

    public TestTypeRepository(RitaContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<ITestTypeModel> GetAll()
    {
        try
        {
            var testTypes = _context.TestTypes;

            return testTypes?.ToITestTypeModelEnumerable();

        }
        catch (Exception e)
        {
            _logger.Error(e.Message, e);
            throw new RepositoryException(e.Message, e);
        }
    }
}