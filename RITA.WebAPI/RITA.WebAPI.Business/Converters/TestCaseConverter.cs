using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Models;
using RITA.WebAPI.Business.Views;

namespace RITA.WebAPI.Business.Converters;

public static class TestCaseConverter
{
    public static ITestCaseModel ToModel(this ITestCaseView view)
    {
        return new TestCaseModel()
        {
            Id = view.Id,
            SuiteId = view.SuiteId,
            Url = view.Url,
            RequestMethod = view.RequestMethod,
            Name = view.Name,
        };

    }

    public static ITestCaseView ToView(this ITestCaseModel model)
    {
        return new TestCaseView()
        {
            Id = model.Id,
            SuiteId = model.SuiteId,
            Url = model.Url,
            RequestMethod = model.RequestMethod,
            Name = model.Name,
        };
    }

    public static List<ITestCaseView> ToViews(this IEnumerable<ITestCaseModel> models)
    {
        return models.Select(model => model.ToView()).ToList();
    }
}