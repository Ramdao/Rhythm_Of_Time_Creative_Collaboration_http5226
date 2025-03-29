namespace Rhythm_Of_Time.Models
{
    public class EntryDetails
    {
        public EntryDto Entry { get; set; }
        public TimelineDto Timeline { get; set; }
        public IEnumerable<EntryDto> AssociatedEntries { get; set; }
        public IEnumerable<SongDTO> AssociatedSongs { get; set; }
    }
}
