using RITA.WebAPI.Abstractions.Models;

namespace RITA.WebAPI.Repository.Models
{
    public class FavoriteModel : IFavoriteModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string FavoriteType { get; set; }
        public int ReferenceId { get; set; }
    }
}
