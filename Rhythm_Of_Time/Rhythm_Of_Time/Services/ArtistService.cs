using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ApplicationDbContext _context;

        public ArtistService(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all artists with mapping to ArtistDto
        public async Task<IEnumerable<ArtistDto>> List()
        {
            // Retrieve all artists from the database
            var artists = await _context.artist.ToListAsync();

            // Map each Artist entity to an ArtistDto
            return artists.Select(a => new ArtistDto
            {
                ArtistId = a.ArtistId,
                name = a.name,
                nationality = a.nationality
            }).ToList();
        }

        // Find an artist by ID and return an ArtistDto
        public async Task<ArtistDto?> FindArtist(int id)
        {
            var artist = await _context.artist
                                       .Where(a => a.ArtistId == id)
                                       .FirstOrDefaultAsync();

            if (artist == null) return null;

            // Map to ArtistDto before returning
            return new ArtistDto
            {
                ArtistId = artist.ArtistId,
                name = artist.name,
                nationality = artist.nationality
            };
        }

        // Update an artist's information
        public async Task<ServiceResponse> UpdateArtist(int id, ArtistDto artistDto)
        {
            ServiceResponse serviceResponse = new();

            var existingArtist = await _context.artist.FindAsync(id);
            if (existingArtist == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Artist not found.");
                return serviceResponse;
            }

            // Update artist details using ArtistDto
            existingArtist.name = artistDto.name;
            existingArtist.nationality = artistDto.nationality;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error updating artist.");
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }

        // Add a new artist
        public async Task<ServiceResponse> AddArtist(ArtistDto artistDto)
        {
            ServiceResponse serviceResponse = new();

            if (string.IsNullOrWhiteSpace(artistDto.name) || string.IsNullOrWhiteSpace(artistDto.nationality))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Artist name and nationality are required.");
                return serviceResponse;
            }

            // Convert ArtistDto to Artist
            var artist = new Artist
            {
                name = artistDto.name,
                nationality = artistDto.nationality
            };

            _context.artist.Add(artist);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error adding artist.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = artist.ArtistId;
            return serviceResponse;
        }

        // Delete an artist by ID
        public async Task<ServiceResponse> DeleteArtist(int id)
        {
            ServiceResponse serviceResponse = new();

            // Find the artist to delete
            var artist = await _context.artist.FindAsync(id);
            if (artist == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Artist not found.");
                return serviceResponse;
            }

            try
            {
                // Remove the artist from the database
                _context.artist.Remove(artist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error deleting artist.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }
    }
}
