using RITA.EF;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Repository;
using RITA.WebAPI.Abstractions.Repository.Validation;
using RITA.WebAPI.Abstractions.Validation;
using RITA.WebAPI.Core.Models;
using RITA.WebAPI.Repository.Converters;
using RockLib.Logging;

namespace RITA.WebAPI.Repository.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ILogger _logger;
        private readonly RitaContext _context;
        private readonly IFavoriteModelValidator _validator;

        public FavoriteRepository(RitaContext context, IFavoriteModelValidator validator, ILogger logger)
        {
            _context = context;
            _validator = validator;
            _logger = logger;
        }
        public int Delete(int id)
        {
            try
            {
                var favorite = _context.Favorites.Find(id);
                if (favorite == null) return id;
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();

                return id;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new RepositoryException(e.Message, e);
            }
        }

        public IEnumerable<IFavoriteModel> GetFavoritesByUserId(string userId)
        {
            try
            {
                if (_context.Favorites == null)
                    return Enumerable.Empty<IFavoriteModel>();
                var favorites = _context.Favorites.Where(f => f.UserId == userId);
                return favorites.ToIFavoriteModelEnumerable();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new RepositoryException(e.Message, e);
            }
        }

        public IFavoriteModel Insert(IFavoriteModel entity)
        {
            try
            {
                _validator.ValidateInsert(entity);

                var favorite = entity.ToFavorite();
                _context.Favorites.Add(favorite);
                _context.SaveChanges();

                return favorite.ToIFavoriteModel();
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                throw new RepositoryException(e.Message, e);
            }
        }
    }
}
