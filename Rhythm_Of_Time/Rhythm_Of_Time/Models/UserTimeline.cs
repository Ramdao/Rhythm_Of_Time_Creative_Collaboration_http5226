using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rhythm_Of_Time.Models
{
    public class UserTimeline
    {
        [Key]
        public int usertime_Id { get; set; }

        // Foreign key to the Timeline table
        [ForeignKey("Timeline")]
        public int timeline_Id { get; set; }
        public virtual Timeline Timeline { get; set; }

        // Foreign key to the IdentityUser table
        public string user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual IdentityUser User { get; set; }
    }

    public class UserTimelineDto
    {
        public int usertime_Id { get; set; }

        public int timeline_Id { get; set; }

        public string user_id { get; set; }

        public string timeline_name { get; set; }

        public string user_email { get; set; }

        public string user_name { get; set; }

        public int TimelineId { get; set; }
    }
}
