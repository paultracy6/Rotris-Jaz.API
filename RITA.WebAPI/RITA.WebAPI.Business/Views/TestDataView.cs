using RITA.WebAPI.Abstractions.Views;
using System.Net;

namespace RITA.WebAPI.Business.Views;
public class TestDataView : ITestDataView
{
    public int Id { get; set; }
    public int TestTypeId { get; set; }
    public int TestCaseId { get; set; }
    public string Name { get; set; }
    public bool IsSuspended { get; set; }
    public string Comment { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public object RequestContent { get; set; }
    public int RequestContentTypeId { get; set; }
    public object ResponseContent { get; set; }
    public int ResponseContentTypeId { get; set; }
}