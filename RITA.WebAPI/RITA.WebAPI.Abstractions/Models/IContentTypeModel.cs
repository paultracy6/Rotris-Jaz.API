namespace RITA.WebAPI.Abstractions.Models;

public interface IContentTypeModel : ICommonModel
{
    string MimeType { get; set; }
}