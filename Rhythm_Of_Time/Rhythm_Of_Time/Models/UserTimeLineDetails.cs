namespace Rhythm_Of_Time.Models
{
    public class UserTimeLineDetails
    {
        public IEnumerable<UserDto> Users { get; set; }
        public IEnumerable<TimelineDto> Timelines { get; set; }
        public IEnumerable<UserTimelineDto> UserTimelines { get; set; }

        public string UserId { get; set; }
    }
}
