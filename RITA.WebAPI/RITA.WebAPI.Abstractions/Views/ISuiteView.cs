namespace RITA.WebAPI.Abstractions.Views;

public interface ISuiteView : ICommonView
{
    int AppId { get; set; }
    string Name { get; set; }
}