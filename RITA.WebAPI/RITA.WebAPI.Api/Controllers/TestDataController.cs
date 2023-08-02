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
    [Route("api/test-data")]
    [ApiController]
    public class TestDataController : BaseController
    {
        private readonly ITestDataService _testDataService;
        private readonly ITestDataControllerValidator _validator;
        private readonly ILogger _logger;

        public TestDataController(ITestDataService testDataService, ITestDataControllerValidator validator, ILogger logger)
        {
            _testDataService = testDataService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_testDataService.GetById(id));
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

        [HttpPost()]
        public IActionResult Post([FromBody] TestData testData)
        {
            try
            {
                _validator.ValidateInsert(testData);
                return Ok(_testDataService.Insert(testData, "API")); //ToDo: Get from token
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
        public IActionResult Put(int id, [FromBody] TestData testData)
        {
            try
            {
                _validator.ValidateUpdate(id, testData);
                return Ok(_testDataService.Update(testData, "API")); //ToDo: Get from token
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
                return Ok(_testDataService.Delete(id));
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
