namespace Rhythm_Of_Time.Models
{
    public class SuperUserModel
    {
        public UserDto User { get; set; }
        public IEnumerable<TimelineDetailsForUser> Timelines { get; set; }
    }

   

    
}
