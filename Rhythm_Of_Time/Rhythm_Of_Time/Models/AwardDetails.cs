namespace Rhythm_Of_Time.Models
{
    public class AwardDetails
    {
        public AwardDto Award { get; set; }
        public IEnumerable<SongDTO> Songs { get; set; }
        public IEnumerable<ArtistDto> Artists { get; set; }
    }
}
