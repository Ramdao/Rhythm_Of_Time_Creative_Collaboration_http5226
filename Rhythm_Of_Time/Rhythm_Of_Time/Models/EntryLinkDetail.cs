namespace Rhythm_Of_Time.Models
{
    public class EntryLinkDetail
    {
        public IEnumerable<SongDTO> Songs { get; set; }
        public IEnumerable<TimelineDto> Timelines { get; set; }
        public IEnumerable<UserTimelineDto> UserTimelines { get; set; }

        public string SongId { get; set; }
    }
}
