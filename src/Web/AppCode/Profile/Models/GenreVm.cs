
namespace Models.Profile
{

    public class FavGenreVm
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsFavorite { get; set; }
    }

}
