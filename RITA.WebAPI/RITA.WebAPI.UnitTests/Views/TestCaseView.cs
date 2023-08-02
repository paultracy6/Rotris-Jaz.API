using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.UnitTests.Views;

public class TestCaseView : ITestCaseView
{
    public int Id { get; set; }
    public int SuiteId { get; set; }
    public string Url { get; set; }
    public string RequestMethod { get; set; }
    public string Name { get; set; }
}