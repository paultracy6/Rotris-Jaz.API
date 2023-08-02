using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.UnitTests.Views;

public class TestTypeView : ITestTypeView
{
    public int Id { get; set; }
    public string Name { get; set; }
}