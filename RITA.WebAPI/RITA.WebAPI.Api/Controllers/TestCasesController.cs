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
    [Route("api/test-cases")]
    [ApiController]
    public class TestCasesController : BaseController
    {
        private readonly ITestCaseService _testCaseService;
        private readonly ITestCaseControllerValidator _validator;
        private readonly ILogger _logger;

        public TestCasesController(ITestCaseService testCaseService, ITestCaseControllerValidator validator, ILogger logger)
        {
            _testCaseService = testCaseService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_testCaseService.GetById(id));
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
        public IActionResult Post([FromBody] TestCase testCase)
        {
            try
            {
                _validator.ValidateInsert(testCase);
                return Ok(_testCaseService.Insert(testCase, "API"));
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
        public IActionResult Put(int id, [FromBody] TestCase testCase)
        {
            try
            {
                _validator.ValidateUpdate(id, testCase);
                return Ok(_testCaseService.Update(testCase, "API")); //ToDo: Get from token
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
                return Ok(_testCaseService.Delete(id));
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

        [HttpGet("{id:int}/test-data")]
        public IActionResult GetTestData(int id)
        {
            try
            {
                return Ok(_testCaseService.GetTestData(id));
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
