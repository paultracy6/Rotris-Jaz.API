using RITA.WebAPI.Abstractions.Models;
using System.Net;

namespace RITA.WebAPI.UnitTests.Models;

public class TestDataModel : ITestDataModel
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public int TestTypeId { get; set; }
    public int TestCaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSuspended { get; set; }
    public DateTime? SuspendedOn { get; set; }
    public string? SuspendedBy { get; set; }
    public string? Comment { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public object RequestContent { get; set; } = new object();
    public int RequestContentTypeId { get; set; }
    public object ResponseContent { get; set; } = new object();
    public int ResponseContentTypeId { get; set; }
}