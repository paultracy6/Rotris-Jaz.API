using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Abstractions.Repository;

public interface IContentTypeRepository : IBaseRepository<IContentTypeModel>
{
    IEnumerable<IContentTypeModel> GetAll();

}