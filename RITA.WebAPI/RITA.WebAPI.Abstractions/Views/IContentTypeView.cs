namespace RITA.WebAPI.Abstractions.Views;

public interface IContentTypeView : ICommonView
{
    string MimeType { get; set; }
}