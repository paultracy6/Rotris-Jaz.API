namespace RITA.WebAPI.Abstractions.Views
{
    public interface IFavoriteView
    {
        int Id { get; set; }
        string UserId { get; set; }
        string Name { get; set; }
        string FavoriteType { get; set; }
        int ReferenceId { get; set; }
    }
}
