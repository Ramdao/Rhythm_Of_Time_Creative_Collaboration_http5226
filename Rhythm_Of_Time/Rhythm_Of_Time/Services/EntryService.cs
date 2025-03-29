using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Rhythm_Of_Time.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Rhythm_Of_Time.Services
{
    public class EntryService : IEntryService
    {
        private readonly ApplicationDbContext _context;

        public EntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all entries for a specific timeline
        public async Task<IEnumerable<EntryDto>> GetEntriesForTimeline(int timelineId)
        {
            var entries = await _context.entry
                .Where(e => e.timeline_Id == timelineId)
                .Join(_context.song,
                      e => e.SongId,
                      s => s.SongId,
                      (e, s) => new EntryDto
                      {
                          entry_Id = e.entry_Id,
                          timeline_Id = e.timeline_Id,
                          SongId = s.SongId,
                          decription = e.decription
                      })
                .ToListAsync();

            return entries;
        }

        // Get all entries for a specific song
        public async Task<IEnumerable<EntryDto>> GetEntriesForSong(int songId)
        {
            var entries = await _context.entry
                .Where(e => e.SongId == songId)
                .Join(_context.timelines,
                      e => e.timeline_Id,
                      t => t.timeline_Id,
                      (e, t) => new EntryDto
                      {
                          entry_Id = e.entry_Id,
                          timeline_Id = e.timeline_Id,
                          SongId = e.SongId,
                          decription = e.decription
                      })
                .ToListAsync();

            return entries;
        }

        // Link a song to a timeline (create an entry)
        public async Task<ServiceResponse> LinkEntryToTimelineAndSong(int timelineId, int songId, string description)
        {
            var serviceResponse = new ServiceResponse();

            // Check if the song and timeline exist
            var timelineExists = await _context.timelines.AnyAsync(t => t.timeline_Id == timelineId);
            var songExists = await _context.song.AnyAsync(s => s.SongId == songId);

            if (!timelineExists || !songExists)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Timeline or Song not found.");
                return serviceResponse;
            }

            // Check if the entry already exists to avoid duplication
            var entryExists = await _context.entry
                .AnyAsync(e => e.timeline_Id == timelineId && e.SongId == songId);

            if (entryExists)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("This song is already linked to the specified timeline.");
                return serviceResponse;
            }

            // Create the new entry
            var entry = new Entry
            {
                timeline_Id = timelineId,
                SongId = songId,
                decription = description
            };

            try
            {
                _context.entry.Add(entry);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                serviceResponse.CreatedId = entry.entry_Id;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error while linking the song to the timeline.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        // Unlink a song from a timeline (remove the entry)
        public async Task<ServiceResponse> UnlinkEntry(int timelineId, int songId)
        {
            var serviceResponse = new ServiceResponse();

            // Find the entry
            var entry = await _context.entry
                .FirstOrDefaultAsync(e => e.timeline_Id == timelineId && e.SongId == songId);

            if (entry == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("No entry found to unlink.");
                return serviceResponse;
            }

            try
            {
                _context.entry.Remove(entry);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                serviceResponse.Messages.Add("Entry successfully removed.");
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error while unlinking the song from the timeline.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        // Update an existing entry (change description or other details)
        public async Task<ServiceResponse> UpdateEntry(int entryId, EntryDto updatedEntryDto)
        {
            var serviceResponse = new ServiceResponse();

            // Find the existing entry
            var entry = await _context.entry.FindAsync(entryId);
            if (entry == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Entry not found.");
                return serviceResponse;
            }

            // Update the entry details
            entry.decription = updatedEntryDto.decription;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error updating entry.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<EntryDto?> FindEntry(int id)
        {
            var entry = await _context.entry.FirstOrDefaultAsync(s => s.entry_Id== id);

            if (entry == null)
            {
                return null;
            }

            return new EntryDto()
            {
                entry_Id = entry.entry_Id,
                timeline_Id = entry.timeline_Id,
                SongId = entry.SongId,
                decription = entry.decription

            };
        }
    }
}
