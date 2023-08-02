using RITA.WebAPI.Abstractions.Views;

namespace RITA.WebAPI.UnitTests.Views
{
    public class FavoriteView : IFavoriteView
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string FavoriteType { get; set; }
        public int ReferenceId { get; set; }
    }
}
