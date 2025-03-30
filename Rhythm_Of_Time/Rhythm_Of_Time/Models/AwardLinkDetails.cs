namespace Rhythm_Of_Time.Models
{
    public class AwardLinkDetails
    {
        public IEnumerable<SongDTO> Songs { get; set; }
        public IEnumerable<AwardDto> Awards { get; set; }
        public IEnumerable<AwardSongDto> AwardSong { get; set; }

        public string SongId { get; set; }
    }
}
