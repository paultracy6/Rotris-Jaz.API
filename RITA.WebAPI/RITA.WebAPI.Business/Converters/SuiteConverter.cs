using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Models;
using RITA.WebAPI.Business.Views;

namespace RITA.WebAPI.Business.Converters;

public static class SuiteConverter
{
    public static ISuiteModel ToModel(this ISuiteView view)
    {
        return new SuiteModel()
        {
            AppId = view.AppId,
            Id = view.Id,
            Name = view.Name
        };

    }

    public static ISuiteView ToView(this ISuiteModel model)
    {
        return new SuiteView()
        {
            AppId = model.AppId,
            Id = model.Id,
            Name = model.Name
        };

    }

    public static List<ISuiteView> ToViews(this IEnumerable<ISuiteModel> models)
    {
        return models.Select(model => model.ToView()).ToList();
    }

}