using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Abstractions.Services
{
    public interface IFavoriteService
    {
        IEnumerable<IFavoriteView>? GetAllByUserId(string userId);

        IFavoriteView Insert(IFavoriteView view);

        int Delete(int id);
    }
}
