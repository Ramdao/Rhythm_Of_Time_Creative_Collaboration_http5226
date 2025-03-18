using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Rhythm_Of_Time.Services
{
    public class SongService : ISongService
    {
        private readonly ApplicationDbContext _context;

        public SongService(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all songs
        public async Task<IEnumerable<SongDTO>> List()
        {
            var songs = await _context.song.ToListAsync();
            List<SongDTO> songDtos = new List<SongDTO>();

            foreach (var song in songs)
            {
                songDtos.Add(new SongDTO()
                {
                    SongId = song.SongId,
                    Title = song.Title,
                    Album = song.Album,
                    ReleaseYear = song.ReleaseYear,
                   
                });
            }

            return songDtos;
        }

        // Find a song by ID
        public async Task<SongDTO?> FindSong(int id)
        {
            var song = await _context.song.FirstOrDefaultAsync(s => s.SongId == id);

            if (song == null)
            {
                return null;
            }

            return new SongDTO()
            {
                SongId = song.SongId,
                Title = song.Title,
                Album = song.Album,
                ReleaseYear = song.ReleaseYear,    
               
            };
        }

        // Update song
        public async Task<ServiceResponse> UpdateSong(int id, SongDTO songDto)
        {
            ServiceResponse serviceResponse = new();

            if (string.IsNullOrWhiteSpace(songDto.Title))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Song title is required.");
                return serviceResponse;
            }

            var song = await _context.song.FindAsync(id);
            if (song == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Song not found.");
                return serviceResponse;
            }

            // Update song properties
            song.Title = songDto.Title;
            song.Album = songDto.Album;
            song.ReleaseYear = songDto.ReleaseYear;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error updating song.");
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }

        // Add a new song
        public async Task<ServiceResponse> AddSong(SongDTO songDto)
        {
            ServiceResponse serviceResponse = new();

            if (string.IsNullOrWhiteSpace(songDto.Title))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Song title is required.");
                return serviceResponse;
            }

            Song song = new Song()
            {
                Title = songDto.Title,
                Album = songDto.Album,
                ReleaseYear = songDto.ReleaseYear
            };

            try
            {
                _context.song.Add(song);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the song.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = song.SongId;
            return serviceResponse;
        }

        // Delete song
        public async Task<ServiceResponse> DeleteSong(int id)
        {
            ServiceResponse response = new();

            var song = await _context.song.FindAsync(id);

            if (song == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Song not found.");
                return response;
            }

            try
            {
                _context.song.Remove(song);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the song.");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            return response;
        }
    }
}
