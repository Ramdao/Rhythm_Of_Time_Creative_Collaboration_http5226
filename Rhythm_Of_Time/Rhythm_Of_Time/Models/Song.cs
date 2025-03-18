using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhythm_Of_Time.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }

        public string? Title { get; set; }
        public string Album { get; set; }

        public int ReleaseYear { get; set; }


    }

    public class SongDTO
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public int ReleaseYear { get; set; }
      
    }
}
