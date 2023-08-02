using RITA.EF.Models;
using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Repository.Models;
using System.Reflection;

namespace RITA.WebAPI.Repository.Converters
{
    internal static class FavoriteConverter
    {
        public static Favorite ToFavorite(this IFavoriteModel favoriteModel)
        {
            var favorite = new Favorite();

            PropertyInfo[] propertyInfo = favorite.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                var fm = favoriteModel.GetType().GetProperty(property.Name);
                if (fm != null)
                    property.SetValue(favorite, fm.GetValue(favoriteModel));
            }

            return favorite;
        }

        public static IFavoriteModel ToIFavoriteModel(this Favorite favorite)
        {
            var favoriteModel = new FavoriteModel();

            PropertyInfo[] propertyInfo = favoriteModel.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                var f = favorite.GetType().GetProperty(property.Name);
                if (f != null)
                    property.SetValue(favoriteModel, f.GetValue(favorite));
            }

            return favoriteModel;
        }

        public static IEnumerable<IFavoriteModel> ToIFavoriteModelEnumerable(this IQueryable<Favorite> favorites)
        {
            var models = new List<IFavoriteModel>();
            foreach (Favorite favorite in favorites)
            {
                models.Add(favorite.ToIFavoriteModel());
            }

            return models;
        }
    }
}
