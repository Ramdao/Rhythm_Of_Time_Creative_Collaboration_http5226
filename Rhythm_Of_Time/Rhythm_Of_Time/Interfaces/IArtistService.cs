using Rhythm_Of_Time.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IArtistService
    {
        // List all artists
        Task<IEnumerable<Artist>> List();

        // Find an artist by their ID
        Task<Artist?> FindArtist(int id);

        // Update an artist's information
        Task<ServiceResponse> UpdateArtist(int id, Artist artist);

        // Add a new artist
        Task<ServiceResponse> AddArtist(Artist artist);

        // Delete an artist by their ID
        Task<ServiceResponse> DeleteArtist(int id);
    }
}
