using System.Net;

namespace RITA.WebAPI.Abstractions.Models
{
    public interface ITestDataModel : ICommonModel
    {
        int TestTypeId { get; set; }
        int TestCaseId { get; set; }
        string Name { get; set; }
        bool IsSuspended { get; set; }
        DateTime? SuspendedOn { get; set; }
        string? SuspendedBy { get; set; }
        string? Comment { get; set; }
        HttpStatusCode StatusCode { get; set; }
        object RequestContent { get; set; }
        int RequestContentTypeId { get; set; }
        object ResponseContent { get; set; }
        int ResponseContentTypeId { get; set; }
    }
}
