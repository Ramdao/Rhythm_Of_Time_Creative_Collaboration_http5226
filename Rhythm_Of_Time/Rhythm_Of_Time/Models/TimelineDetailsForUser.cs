namespace Rhythm_Of_Time.Models
{
    public class TimelineDetailsForUser
    {
        public TimelineDto Timeline { get; set; }
        public IEnumerable<SuperEntryDetails> Entries { get; set; }
    }
}
