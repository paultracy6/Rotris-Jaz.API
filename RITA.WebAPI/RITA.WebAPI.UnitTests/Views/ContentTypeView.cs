using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.UnitTests.Views;

public class ContentTypeView : IContentTypeView
{
    public int Id { get; set; }
    public string MimeType { get; set; }
}