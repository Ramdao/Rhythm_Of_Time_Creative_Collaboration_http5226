using System.ComponentModel.DataAnnotations;

namespace Rhythm_Of_Time.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        public string? name { get; set; }
        public string nationality { get; set; }

      
    }
    public class ArtistDto
    {

        public int ArtistId { get; set; }

        public string? name { get; set; }
        public string nationality { get; set; }


    }
}
