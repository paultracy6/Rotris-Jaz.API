namespace RITA.WebAPI.Abstractions.Views;

public interface ITestCaseView : ICommonView
{
    int SuiteId { get; set; }
    string Url { get; set; }
    string RequestMethod { get; set; }
    string Name { get; set; }
}