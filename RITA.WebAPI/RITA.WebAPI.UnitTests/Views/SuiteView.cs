using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.UnitTests.Views;

public class SuiteView : ISuiteView
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Name { get; set; }
}