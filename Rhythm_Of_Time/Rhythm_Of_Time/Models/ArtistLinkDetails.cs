namespace Rhythm_Of_Time.Models
{
    public class ArtistLinkDetails
    {

        public IEnumerable<SongDTO> Songs { get; set; }
        public IEnumerable<ArtistDto> Artists { get; set; }
        public IEnumerable<ArtistSongDto> ArtistSong { get; set; }

        public string SongId { get; set; }
    }
}
