using RITA.WebAPI.Abstractions.Views;
using System.Net;

namespace RITA.WebAPI.Api.Models
{
    public class TestData : ITestDataView
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int TestTypeId { get; set; }
        public int TestCaseId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSuspended { get; set; }
        public DateTime? SuspendedOn { get; set; }
        public string SuspendedBy { get; set; }
        public string Comment { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object RequestContent { get; set; }
        public int RequestContentTypeId { get; set; }
        public object ResponseContent { get; set; }
        public int ResponseContentTypeId { get; set; }
    }
}
