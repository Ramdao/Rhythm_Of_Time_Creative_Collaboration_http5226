using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rhythm_Of_Time.Models
{
    public class AwardSong
    {
        [Key]
        public int AwardSong_Id { get; set; }

        // Foreign key to the Timeline table
        [ForeignKey("Song")]
        public int SongId { get; set; }

        public virtual Song Song { get; set; }

        // Foreign key to the IdentityUser table
        [ForeignKey("Award")]
        public int AwardId { get; set; }

        public virtual Award Award { get; set; }

        public string status { get; set; }
    }

    public class AwardSongDto
    {
        public int AwardSong_Id { get; set; }
        public int SongId { get; set; }
        public int AwardId { get; set; }
        public string AwardName { get; set; }  
        public int AwardYear { get; set; } 
        public string Status { get; set; }


    }
}

