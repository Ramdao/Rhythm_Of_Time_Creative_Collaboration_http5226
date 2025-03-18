using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IArtistSongService
    {
        Task<ServiceResponse> LinkArtistToSong(int songId, int artistId, string role);
        Task<ServiceResponse> UnlinkArtistFromSong(int songId, int artistId);
        Task<IEnumerable<ArtistSongDto>> GetArtistsForSong(int songId);
        Task<IEnumerable<ArtistSongDto>> GetSongsForArtist(int artistId);
    }
}
