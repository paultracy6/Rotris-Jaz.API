using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Models;
using RITA.WebAPI.Business.Views;
using System.Reflection;

namespace RITA.WebAPI.Business.Converters
{
    public static class FavoriteConverter
    {
        public static IFavoriteView ToView(this IFavoriteModel model)
        {
            var view = new FavoriteView();

            PropertyInfo[] propertyInfo = view.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                var modelProperty = model.GetType().GetProperty(property.Name);
                if (modelProperty != null)
                    property.SetValue(view, modelProperty.GetValue(model));
            }

            return view;
        }

        public static IFavoriteModel ToModel(this IFavoriteView view)
        {
            var model = new FavoriteModel();

            PropertyInfo[] propertyInfo = model.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfo)
            {
                var viewProperty = view.GetType().GetProperty(property.Name);
                if (viewProperty != null)
                    property.SetValue(model, viewProperty.GetValue(view));
            }
            return model;
        }

        public static IEnumerable<IFavoriteView> ToViews(this IEnumerable<IFavoriteModel> models)
        {
            var views = new List<IFavoriteView>();
            foreach (IFavoriteModel model in models)
                views.Add(ToView(model));
            return views;
        }
    }
}
