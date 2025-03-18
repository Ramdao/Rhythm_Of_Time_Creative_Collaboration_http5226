using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IAwardSongService
    {
        Task<ServiceResponse> LinkAwardToSong(int songId, int awardId, string status);
        Task<ServiceResponse> UnlinkAwardFromSong(int songId, int awardId);
        Task<IEnumerable<AwardSongDto>> GetAwardsForSong(int songId);
        Task<IEnumerable<AwardSongDto>> GetSongsForAward(int awardId);
    }
}
