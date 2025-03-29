namespace Rhythm_Of_Time.Models
{
    public class UserDetails
    {
        public required UserDto User { get; set; }

        public IEnumerable<UserTimelineDto>? UserTimeline { get; set; }

       

       
    }
}
