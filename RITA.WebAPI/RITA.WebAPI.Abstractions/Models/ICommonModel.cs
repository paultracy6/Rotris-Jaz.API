namespace RITA.WebAPI.Abstractions.Models
{
    public interface ICommonModel
    {
        int Id { get; set; }
        DateTime CreatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        string? UpdatedBy { get; set; }
    }
}
