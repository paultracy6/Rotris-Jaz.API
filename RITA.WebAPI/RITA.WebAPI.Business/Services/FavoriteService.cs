using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Services;
using RITA.WebAPI.Abstractions.Services.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Converters;
using RITA.WebAPI.Core.Models;
using RockLib.Logging;

namespace RITA.WebAPI.Business.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IFavoriteValidator _favoriteValidator;
        private readonly ILogger _logger;

        public FavoriteService(IFavoriteRepository favoriteRepository, IFavoriteValidator favoriteValidator, ILogger logger)
        {
            _favoriteRepository = favoriteRepository;
            _favoriteValidator = favoriteValidator;
            _logger = logger;
        }

        public IEnumerable<IFavoriteView>? GetAllByUserId(string userId)
        {
            try
            {
                IEnumerable<IFavoriteModel>? response = _favoriteRepository.GetFavoritesByUserId(userId);

                return response?.ToViews();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new Exception(e.Message);
            }
        }

        public IFavoriteView Insert(IFavoriteView view)
        {
            try
            {
                IFavoriteModel model = view.ToModel();
                _favoriteValidator.ValidateInsert(model);
                IFavoriteModel response = _favoriteRepository.Insert(model);
                return response.ToView();
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw new BusinessException(e.Message);
            }
        }

        public int Delete(int id)
        {
            try
            {
                return _favoriteRepository.Delete(id);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw new BusinessException(e.Message);
            }
        }
    }
}
