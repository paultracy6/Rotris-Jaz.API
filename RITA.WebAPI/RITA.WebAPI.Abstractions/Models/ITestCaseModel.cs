namespace RITA.WebAPI.Abstractions.Models
{
    public interface ITestCaseModel : ICommonModel
    {
        int SuiteId { get; set; }
        string Url { get; set; }
        string RequestMethod { get; set; }
        string Name { get; set; }
    }
}
