using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rhythm_Of_Time.Models
{
    public class Entry
    {
        [Key]
        public int entry_Id { get; set; }

        // Foreign key to the Timeline table
        [ForeignKey("Timeline")]
        public int timeline_Id { get; set; }
        public virtual Timeline Timeline { get; set; }

        // Foreign key to the IdentityUser table
        [ForeignKey("Song")]
        public int SongId { get; set; }
       
        public virtual Song Song { get; set; }

        public string decription { get; set; }
    }

    public class EntryDto
    {
        public int entry_Id { get; set; }
        public int timeline_Id { get; set; }
        public int SongId { get; set; }

        public string decription { get; set; }
    }
   
}
