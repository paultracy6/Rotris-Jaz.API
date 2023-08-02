using Microsoft.AspNetCore.Mvc;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;
using ILogger = RockLib.Logging.ILogger;

namespace RITA.WebAPI.Api.Controllers
{
    [Route("api/test-types")]
    [ApiController]
    public class TestTypesController : BaseController
    {
        private readonly ITestTypeService _testTypeService;
        private readonly ILogger _logger;

        public TestTypesController(ITestTypeService testTypeService, ILogger logger)
        {
            _testTypeService = testTypeService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetTestTypeAll()
        {
            try
            {
                return Ok(_testTypeService.GetAll());
            }
            catch (BusinessException e)
            {
                return BusinessErrorResponse(e);
            }
            catch (RepositoryException e)
            {
                return RepositoryErrorResponse(e);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                return ControllerErrorResponse(e);
            }
        }


    }
}
