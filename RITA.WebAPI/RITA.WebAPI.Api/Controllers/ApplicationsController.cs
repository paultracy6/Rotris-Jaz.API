using Microsoft.AspNetCore.Mvc;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;
using ILogger = RockLib.Logging.ILogger;

namespace RITA.WebAPI.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController : BaseController
	{
		private readonly IApplicationService _applicationService;
		private readonly ILogger _logger;

		public ApplicationController(IApplicationService applicationService,ILogger logger)
		{
			_applicationService = applicationService;
			_logger = logger;
		}

		[HttpGet("{id}/suites")]
		public IActionResult GetSuites(int id)
		{
			try
			{
				return Ok(_applicationService.GetSuiteByAppId(id));
			}
			catch(BusinessException e)
			{
				return BusinessErrorResponse(e);
			}
			catch(RepositoryException e)
			{
				return RepositoryErrorResponse(e);
			}
			catch(Exception e)
			{
				_logger.Error(e.Message,e,new { id });
				return ControllerErrorResponse(e);
			}
		}

		[HttpGet("{id}/test-cases")]
		public IActionResult GetTestCases(int id)
		{
			try
			{
				return Ok(_applicationService.GetTestCaseByAppId(id));
			}
			catch(BusinessException e)
			{
				return BusinessErrorResponse(e);
			}
			catch(RepositoryException e)
			{
				return RepositoryErrorResponse(e);
			}
			catch(Exception e)
			{
				_logger.Error(e.Message,e,new { id });
				return ControllerErrorResponse(e);
			}
		}
	}
}
