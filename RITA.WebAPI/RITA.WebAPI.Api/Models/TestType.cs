using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Api.Models
{
    public class TestType : ITestTypeView
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
    }
}
