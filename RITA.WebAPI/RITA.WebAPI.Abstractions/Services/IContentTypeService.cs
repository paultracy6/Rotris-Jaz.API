using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.Abstractions.Services;

public interface IContentTypeService
{
    IEnumerable<IContentTypeView> GetAll();

}