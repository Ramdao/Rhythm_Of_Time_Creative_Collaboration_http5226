namespace Rhythm_Of_Time.Models
{
    public class SongDetails
    {

        public required SongDTO Song { get; set; }
        public IEnumerable<ArtistDto>? Artists { get; set; }
        public IEnumerable<AwardDto>? Awards { get; set; }
    }
}
