using System.Net;

namespace RITA.WebAPI.Abstractions.Views;

public interface ITestDataView : ICommonView
{
    int TestTypeId { get; set; }
    int TestCaseId { get; set; }
    string Name { get; set; }
    bool IsSuspended { get; set; }
    string Comment { get; set; }
    HttpStatusCode StatusCode { get; set; }
    object RequestContent { get; set; }
    int RequestContentTypeId { get; set; }
    object ResponseContent { get; set; }
    int ResponseContentTypeId { get; set; }
}