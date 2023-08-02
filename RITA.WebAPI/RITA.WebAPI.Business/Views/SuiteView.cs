using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Business.Views;

public class SuiteView : ISuiteView
{
    public int Id { get; set; }
    public int AppId { get; set; }
    public string Name { get; set; }
}