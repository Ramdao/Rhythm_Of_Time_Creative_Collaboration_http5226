namespace Rhythm_Of_Time.Models
{
    public class SongDetails
    {

        public required SongDTO Song { get; set; }
        public IEnumerable<ArtistSongDto>? Artists { get; set; }
        public IEnumerable<AwardSongDto>? Awards { get; set; }


    }
}
