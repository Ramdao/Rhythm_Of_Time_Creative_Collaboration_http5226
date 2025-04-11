namespace Rhythm_Of_Time.Models
{
    public class SuperEntryDetails
    {
        public EntryDto Entry { get; set; }
        public SongDTO Song { get; set; }
        public IEnumerable<ArtistDto> Artists { get; set; }
        public IEnumerable<AwardDto> Awards { get; set; }
    }
}
