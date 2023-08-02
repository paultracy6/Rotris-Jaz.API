using Microsoft.AspNetCore.Mvc;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.WebAPI.Validation;
using RITA.WebAPI.Api.Models;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;
using ILogger = RockLib.Logging.ILogger;

namespace RITA.WebAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuiteController : BaseController
    {
        private readonly ISuiteService _suiteService;
        private readonly ISuiteControllerValidator _validator;
        private readonly ILogger _logger;

        public SuiteController(ISuiteService suiteService, ISuiteControllerValidator validator, ILogger logger)
        {
            _suiteService = suiteService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_suiteService.GetById(id));
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
                _logger.Error(e.Message, e, new { id });
                return ControllerErrorResponse(e);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Suite suite)
        {
            try
            {
                _validator.ValidateInsert(suite);
                return Ok(_suiteService.Insert(suite, "API")); //ToDo: Get from token
            }
            catch (ValidationException e)
            {
                return ValidationErrorResponse(e);
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

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Suite suite)
        {
            try
            {
                _validator.ValidateUpdate(id, suite);
                return Ok(_suiteService.Update(suite, "API")); //ToDo: Get from token
            }
            catch (ValidationException e)
            {
                return ValidationErrorResponse(e);
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
                _logger.Error(e.Message, e, new { id });
                return ControllerErrorResponse(e);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_suiteService.Delete(id));
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
                _logger.Error(e.Message, e, new { id });
                return ControllerErrorResponse(e);
            }
        }

        [HttpGet("{id:int}/test-cases")]
        public IActionResult GetTestCases(int id)
        {
            try
            {
                return Ok(_suiteService.GetTestCasesBySuiteId(id));
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
                _logger.Error(e.Message, e, new { id });
                return ControllerErrorResponse(e);
            }
        }

    }

}