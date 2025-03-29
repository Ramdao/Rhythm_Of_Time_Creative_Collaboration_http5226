using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rhythm_Of_Time.Models
{
    public class ArtistSong
    {
        [Key]
        public int ArtistSong_Id { get; set; }

        // Foreign key to the Timeline table
        [ForeignKey("Song")]
        public int SongId { get; set; }

        public virtual Song Song { get; set; }

        // Foreign key to the IdentityUser table
        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        public virtual Artist Artist { get; set; }

        public string role { get; set; }
    }

    public class ArtistSongDto
    {
        public int ArtistSong_Id { get; set; }
        public int SongId { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }  
        public string Role { get; set; }


    }
}

