using Microsoft.AspNetCore.Mvc;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;

namespace RITA.WebAPI.Api.Controllers
{
	public abstract class BaseController : ControllerBase
	{
		public IActionResult ControllerErrorResponse(Exception e)
		{
			return StatusCode((int)StatusCodes.ControllerException,e.Message);
		}

		public IActionResult ValidationErrorResponse(ValidationException exception)
		{
			return BadRequest(exception.GetMessage());
		}

		public IActionResult RepositoryErrorResponse(RepositoryException e)
		{
			return StatusCode((int)StatusCodes.RepositoryException,e.Message);
		}

		public IActionResult BusinessErrorResponse(BusinessException e)
		{
			return StatusCode((int)StatusCodes.BusinessException,e.Message);
		}
	}

	public enum StatusCodes
	{
		RepositoryException = 601,
		BusinessException = 602,
		ControllerException = 603
	}
}
