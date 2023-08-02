namespace RITA.WebAPI.Abstractions.Models
{
    public interface IFavoriteModel
    {
        int Id { get; set; }
        string UserId { get; set; }
        string Name { get; set; }
        string FavoriteType { get; set; }
        int ReferenceId { get; set; }
    }
}
