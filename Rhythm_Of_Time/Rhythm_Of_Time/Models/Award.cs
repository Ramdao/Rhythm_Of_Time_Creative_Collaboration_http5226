using System.ComponentModel.DataAnnotations;

namespace Rhythm_Of_Time.Models
{
    public class Award
    {
        [Key]
        public int AwardId { get; set; }

        public string? name { get; set; }
        public string description { get; set; }
    }
    public class AwardDto
    {
 
        public int AwardId { get; set; }

        public string? name { get; set; }
        public string description { get; set; }
    }
}
