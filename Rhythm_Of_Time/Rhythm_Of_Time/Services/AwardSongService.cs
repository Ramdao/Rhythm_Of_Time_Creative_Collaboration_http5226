using Rhythm_Of_Time.Data;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Microsoft.EntityFrameworkCore;

namespace Rhythm_Of_Time.Services
{
    public class AwardSongService : IAwardSongService
    {
        private readonly ApplicationDbContext _context;

        public AwardSongService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Link an Award to a Song
        public async Task<ServiceResponse> LinkAwardToSong(int songId, int awardId, string status)
        {
            ServiceResponse serviceResponse = new();

            // Check if both Song and Award exist
            var song = await _context.song.FindAsync(songId);
            var award = await _context.award.FindAsync(awardId);

            if (song == null || award == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (song == null) serviceResponse.Messages.Add("Song not found.");
                if (award == null) serviceResponse.Messages.Add("Award not found.");
                return serviceResponse;
            }

            // Check if the relationship already exists
            bool linkExists = await _context.awardSongs.AnyAsync(asg => asg.SongId == songId && asg.AwardId == awardId);
            if (linkExists)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("This song is already linked to the specified award.");
                return serviceResponse;
            }

            try
            {
                // Create a new relationship entry between Song and Award
                AwardSong awardSong = new AwardSong
                {
                    SongId = songId,
                    AwardId = awardId,
                    status = status
                };

                await _context.awardSongs.AddAsync(awardSong);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error occurred while linking award to song.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        // Unlink an Award from a Song
        public async Task<ServiceResponse> UnlinkAwardFromSong(int songId, int awardId)
        {
            ServiceResponse serviceResponse = new();

            var awardSong = await _context.awardSongs
                .FirstOrDefaultAsync(asg => asg.SongId == songId && asg.AwardId == awardId);

            if (awardSong == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("The song is not linked to this award.");
                return serviceResponse;
            }

            try
            {
                _context.awardSongs.Remove(awardSong);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                serviceResponse.Messages.Add("Award successfully unlinked from the song.");
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error occurred while unlinking award from song.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        // Get all Awards for a Song
        public async Task<IEnumerable<AwardSongDto>> GetAwardsForSong(int songId)
        {
            var awardSongs = await _context.awardSongs
                .Where(asg => asg.SongId == songId)
                .Include(asg => asg.Award)
                .Select(asg => new AwardSongDto
                {
                    AwardSong_Id = asg.AwardSong_Id,
                    SongId = asg.SongId,
                    AwardId = asg.AwardId,
                    Status = asg.status
                })
                .ToListAsync();

            return awardSongs;
        }

        // Get all Songs for an Award
        public async Task<IEnumerable<AwardSongDto>> GetSongsForAward(int awardId)
        {
            var awardSongs = await _context.awardSongs
                .Where(asg => asg.AwardId == awardId)
                .Include(asg => asg.Song)
                .Select(asg => new AwardSongDto
                {
                    AwardSong_Id = asg.AwardSong_Id,
                    SongId = asg.SongId,
                    AwardId = asg.AwardId,
                    Status = asg.status
                })
                .ToListAsync();

            return awardSongs;
        }
    }
}
