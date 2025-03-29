namespace Rhythm_Of_Time.Models
{
    public class TimelineDetails
    {
          public required TimelineDto Timeline { get; set; }

         public IEnumerable<EntryDto>? Entries { get; set; }


        public IEnumerable<UserTimelineDto>? UserTimeline { get; set; }
    }
}
