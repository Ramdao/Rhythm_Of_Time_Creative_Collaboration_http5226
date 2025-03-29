using Rhythm_Of_Time.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IArtistService
    {
        // List all artists
        Task<IEnumerable<ArtistDto>> List();

        // Find an artist by their ID
        Task<ArtistDto?> FindArtist(int id);

        // Update an artist's information
        Task<ServiceResponse> UpdateArtist(int id, ArtistDto artist);

        // Add a new artist
        Task<ServiceResponse> AddArtist(ArtistDto artist);

        // Delete an artist by their ID
        Task<ServiceResponse> DeleteArtist(int id);
    }
}
