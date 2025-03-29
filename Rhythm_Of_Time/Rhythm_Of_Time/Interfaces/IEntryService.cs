using Rhythm_Of_Time.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IEntryService
    {
        // Get all entries for a specific timeline
        Task<IEnumerable<EntryDto>> GetEntriesForTimeline(int timelineId);

        // Get all entries for a specific song
        Task<IEnumerable<EntryDto>> GetEntriesForSong(int songId);

        // Link a song to a timeline (create an entry)
        Task<ServiceResponse> LinkEntryToTimelineAndSong(int timelineId, int songId, string description);

        // Unlink a song from a timeline (remove the entry)
        Task<ServiceResponse> UnlinkEntry(int timelineId, int songId);

        // Update an existing entry (change description or other details)
        Task<ServiceResponse> UpdateEntry(int entryId, EntryDto updatedEntryDto);

        Task<EntryDto?> FindEntry(int id);
    }
}
