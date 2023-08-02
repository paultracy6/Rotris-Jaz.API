using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.UnitTests.Models
{
    public class TestCaseModel : ITestCaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public int SuiteId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string RequestMethod { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
