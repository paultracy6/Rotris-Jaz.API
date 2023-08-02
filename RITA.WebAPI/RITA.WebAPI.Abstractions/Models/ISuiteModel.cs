namespace RITA.WebAPI.Abstractions.Models
{
    public interface ISuiteModel : ICommonModel
    {
        int AppId { get; set; }
        string Name { get; set; }
    }
}
