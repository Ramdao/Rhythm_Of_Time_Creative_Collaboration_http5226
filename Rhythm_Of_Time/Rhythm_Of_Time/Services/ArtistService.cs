using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        // List all artists
        public async Task<IEnumerable<Artist>> List()
        {
            // Retrieve all artists from the database
            return await _context.artist.ToListAsync();
        }

        // Find an artist by ID
        public async Task<Artist?> FindArtist(int id)
        {
            // Find the artist with the given ID
            return await _context.artist.FirstOrDefaultAsync(a => a.ArtistId == id);
        }

        // Update an artist's information
        public async Task<ServiceResponse> UpdateArtist(int id, Artist artist)
        {
            ServiceResponse serviceResponse = new();

            // Find the existing artist in the database
            var existingArtist = await _context.artist.FindAsync(id);
            if (existingArtist == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Artist not found.");
                return serviceResponse;
            }

            // Update the artist's details
            existingArtist.name = artist.name;
            existingArtist.nationality = artist.nationality;

            try
            {
                // Save changes to the database
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
        public async Task<ServiceResponse> AddArtist(Artist artist)
        {
            ServiceResponse serviceResponse = new();

            // Validate the data (ensure name and nationality are provided)
            if (string.IsNullOrWhiteSpace(artist.name) || string.IsNullOrWhiteSpace(artist.nationality))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Artist name and nationality are required.");
                return serviceResponse;
            }

            // Add the new artist to the database
            _context.artist.Add(artist);
            try
            {
                // Save changes to the database
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
