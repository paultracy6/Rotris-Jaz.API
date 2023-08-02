using Microsoft.AspNetCore.Mvc;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Abstractions.WebAPI.Validation;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;
using System.Dynamic;
using ILogger = RockLib.Logging.ILogger;

namespace RITA.WebAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : BaseController
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILogger _logger;
        private readonly IFavoriteControllerValidator _validator;
        private const string _controllerName = nameof(FavoritesController);
        private string _methodName = string.Empty;

        public FavoritesController(IFavoriteService favoriteService, IFavoriteControllerValidator validator, ILogger logger)
        {
            _favoriteService = favoriteService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet("{userId:string}")]
        public IActionResult Get(string userId)
        {
            dynamic extendedProperties = new ExpandoObject();
            try
            {
                _methodName = nameof(Get);
                extendedProperties.Method = _methodName;
                extendedProperties.UserId = userId;
                _logger.Info($"{_controllerName}: {_methodName}", extendedProperties as ExpandoObject);

                return Ok(_favoriteService.GetAllByUserId(userId));
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
                extendedProperties.Exception = e;
                _logger.Error(e.Message, e, extendedProperties as ExpandoObject);
                return ControllerErrorResponse(e);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] IFavoriteView favorite)
        {
            try
            {
                _validator.ValidateInsert(favorite);
                return Ok(_favoriteService.Insert(favorite));
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

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_favoriteService.Delete(id));
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
    }
}
