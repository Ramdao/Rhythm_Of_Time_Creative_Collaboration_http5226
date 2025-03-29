using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Interfaces
{
    public interface ISongService
    {
        // List all songs
        Task<IEnumerable<SongDTO>> List();

        // Find a song by its ID
        Task<SongDTO?> FindSong(int id);

        // Update song information
        Task<ServiceResponse> UpdateSong(int id, SongDTO songDto);

        // Add a new song
        Task<ServiceResponse> AddSong(SongDTO songDto);

        // Delete a song by its ID
        Task<ServiceResponse> DeleteSong(int id);

        Task<IEnumerable<SongDTO>> GetSongsForEntry(int entryId);
    }
}
