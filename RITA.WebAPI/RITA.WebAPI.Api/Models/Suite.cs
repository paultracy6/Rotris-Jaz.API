using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Api.Models
{
    public class Suite : ISuiteView
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }
    }
}
