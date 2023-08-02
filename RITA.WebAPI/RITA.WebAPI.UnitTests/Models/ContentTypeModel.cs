using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.UnitTests.Models;

public class ContentTypeModel : IContentTypeModel
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public string MimeType { get; set; } = "application/json";
}