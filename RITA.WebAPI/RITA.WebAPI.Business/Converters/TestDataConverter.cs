using RITA.WebAPI.Abstractions.Models;
using RITA.WebAPI.Abstractions.Views;
using RITA.WebAPI.Business.Models;
using RITA.WebAPI.Business.Views;

namespace RITA.WebAPI.Business.Converters;

public static class TestDataConverter
{
    public static ITestDataModel ToModel(this ITestDataView view)
    {
        return new TestDataModel()
        {
            TestTypeId = view.TestTypeId,
            TestCaseId = view.TestCaseId,
            Name = view.Name,
            IsSuspended = view.IsSuspended,
            Comment = view.Comment,
            StatusCode = view.StatusCode,
            RequestContent = view.RequestContent,
            RequestContentTypeId = view.RequestContentTypeId,
            ResponseContent = view.ResponseContent,
            ResponseContentTypeId = view.ResponseContentTypeId
        };

    }

    public static ITestDataView ToView(this ITestDataModel model)
    {
        return new TestDataView()
        {
            TestTypeId = model.TestTypeId,
            TestCaseId = model.TestCaseId,
            Name = model.Name,
            IsSuspended = model.IsSuspended,
            Comment = model.Comment,
            StatusCode = model.StatusCode,
            RequestContent = model.RequestContent,
            RequestContentTypeId = model.RequestContentTypeId,
            ResponseContent = model.ResponseContent,
            ResponseContentTypeId = model.ResponseContentTypeId
        };
    }

    public static IEnumerable<ITestDataView> ToViews(this IEnumerable<ITestDataModel> models)
    {
        return models.Select(model => model.ToView());
    }

    public static IEnumerable<ITestDataModel> ToModels(this IEnumerable<ITestDataView> views)
    {
        return views.Select(view => view.ToModel());
    }
}