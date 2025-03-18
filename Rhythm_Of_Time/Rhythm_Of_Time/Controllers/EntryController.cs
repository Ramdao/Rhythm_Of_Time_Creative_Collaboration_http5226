using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;

[ApiController]
[Route("api/[controller]")]
public class EntryController : ControllerBase
{
    private readonly IEntryService _entryService;

    public EntryController(IEntryService entryService)
    {
        _entryService = entryService;
    }

    // Link a Song to a Timeline
    [HttpPost("LinkSongToTimeline")]
    public async Task<IActionResult> LinkEntryToTimelineAndSong([FromBody] EntryDto entryDto)
    {
        var response = await _entryService.LinkEntryToTimelineAndSong(entryDto.timeline_Id, entryDto.SongId, entryDto.decription);

        if (response.Status == ServiceResponse.ServiceStatus.Created)
        {
            return CreatedAtAction(nameof(GetEntriesForTimeline), new { timelineId = entryDto.timeline_Id }, entryDto);
        }

        return BadRequest(response);
    }

    // Unlink a Song from a Timeline
    [HttpDelete("UnlinkSongFromTimeline/{timelineId}/{songId}")]
    public async Task<IActionResult> UnlinkEntry(int timelineId, int songId)
    {
        var response = await _entryService.UnlinkEntry(timelineId, songId);
        if (response.Status == ServiceResponse.ServiceStatus.Deleted)
        {
            return NoContent(); // Return No Content to indicate successful deletion
        }
        return BadRequest(response);
    }

    // Get all Entries for a specific Timeline
    [HttpGet("GetEntriesForTimeline/{timelineId}")]
    public async Task<IActionResult> GetEntriesForTimeline(int timelineId)
    {
        var entries = await _entryService.GetEntriesForTimeline(timelineId);
        return Ok(entries);
    }

    // Get all Entries for a specific Song
    [HttpGet("GetEntriesForSong/{songId}")]
    public async Task<IActionResult> GetEntriesForSong(int songId)
    {
        var entries = await _entryService.GetEntriesForSong(songId);
        return Ok(entries);
    }

    // Update an existing Entry
    [HttpPut("UpdateEntry/{entryId}")]
    public async Task<IActionResult> UpdateEntry(int entryId, [FromBody] EntryDto updatedEntryDto)
    {
        var response = await _entryService.UpdateEntry(entryId, updatedEntryDto);
        if (response.Status == ServiceResponse.ServiceStatus.Updated)
        {
            return NoContent();
        }
        return BadRequest(response);
    }
}
