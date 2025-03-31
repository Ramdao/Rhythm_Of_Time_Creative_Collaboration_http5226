using Rhythm_Of_Time.Data;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Microsoft.EntityFrameworkCore;

namespace Rhythm_Of_Time.Services
{
    public class ArtistSongService : IArtistSongService
    {
        private readonly ApplicationDbContext _context;

        public ArtistSongService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Link an Artist to a Song
        public async Task<ServiceResponse> LinkArtistToSong(int songId, int artistId, string role)
        {
            ServiceResponse serviceResponse = new();

            // Check if both Song and Artist exist
            var song = await _context.song.FindAsync(songId);
            var artist = await _context.artist.FindAsync(artistId);

            if (song == null || artist == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (song == null) serviceResponse.Messages.Add("Song not found.");
                if (artist == null) serviceResponse.Messages.Add("Artist not found.");
                return serviceResponse;
            }

            // Check if the relationship already exists
            bool linkExists = await _context.artistSongs.AnyAsync(asg => asg.SongId == songId && asg.ArtistId == artistId);
            if (linkExists)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("This artist is already linked to the specified song.");
                return serviceResponse;
            }

            try
            {
                // Create a new relationship entry between Song and Artist
                ArtistSong artistSong = new ArtistSong
                {
                    SongId = songId,
                    ArtistId = artistId,
                    role = role
                };

                await _context.artistSongs.AddAsync(artistSong);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error occurred while linking artist to song.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        // Unlink an Artist from a Song
        public async Task<ServiceResponse> UnlinkArtistFromSong(int songId, int artistId)
        {
            ServiceResponse serviceResponse = new();

            var artistSong = await _context.artistSongs
                .FirstOrDefaultAsync(asg => asg.SongId == songId && asg.ArtistId == artistId);

            if (artistSong == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("The artist is not linked to this song.");
                return serviceResponse;
            }

            try
            {
                _context.artistSongs.Remove(artistSong);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                serviceResponse.Messages.Add("Artist successfully unlinked from the song.");
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error occurred while unlinking artist from song.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        // Get all Artists for a Song
        public async Task<IEnumerable<ArtistSongDto>> GetSongsForArtist(int artistId)
        {
            var artistSongs = await _context.artistSongs
                .Where(asg => asg.ArtistId == artistId)
                .Include(asg => asg.Song) // Ensures we fetch song details
                .Select(asg => new ArtistSongDto
                {
                    ArtistSong_Id = asg.ArtistSong_Id,
                    SongId = asg.SongId,
                    ArtistId = asg.ArtistId,
                    Title = asg.Song.Title, 
                    Role = asg.role
                })
                .ToListAsync();

            return artistSongs;
        }

        // Get all Songs for an Artist
        public async Task<IEnumerable<ArtistSongDto>> GetArtistsForSong(int songId)
        {
            var artistSongs = await _context.artistSongs
                .Where(asg => asg.SongId == songId)
                .Include(asg => asg.Artist) // Ensures we fetch artist details
                .Select(asg => new ArtistSongDto
                {
                    ArtistSong_Id = asg.ArtistSong_Id,
                    SongId = asg.SongId,
                    ArtistId = asg.ArtistId,
                    ArtistName = asg.Artist.name, // Include artist details
                    Role = asg.role
                })
                .ToListAsync();

            return artistSongs;
        }
    }
}
