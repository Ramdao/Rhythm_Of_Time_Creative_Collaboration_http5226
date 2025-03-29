namespace Rhythm_Of_Time.Models
{
    public class ArtistDetails
    {
        public required ArtistDto Artist { get; set; }
        public IEnumerable<AwardDto>? Awards { get; set; }

        public IEnumerable<SongDTO>? Song { get; set; }
    }
}
