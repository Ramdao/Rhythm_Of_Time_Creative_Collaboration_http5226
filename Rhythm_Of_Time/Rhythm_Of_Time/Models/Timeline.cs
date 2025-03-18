using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhythm_Of_Time.Models
{
    public class Timeline
    {

        [Key]
        public int timeline_Id { get; set; }

        public string timeline_name { get; set; }

        public DateTime date { get; set; }

        public string description { get; set; }
    }

    public class TimelineDto
    {
        public int timeline_Id { get; set; }

        public string timeline_name { get; set; }

        public DateTime date { get; set; }

        public string description { get; set; }



    }
}
