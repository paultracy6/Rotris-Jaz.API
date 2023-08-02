using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Abstractions.Repository
{
    public interface IFavoriteRepository
    {
        IEnumerable<IFavoriteModel> GetFavoritesByUserId(string userId);

        IFavoriteModel Insert(IFavoriteModel favoriteModel);

        int Delete(int id);
    }
}
