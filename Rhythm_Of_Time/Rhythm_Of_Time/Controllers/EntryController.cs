using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Services;

[ApiController]
[Route("api/[controller]")]
public class EntryController : ControllerBase
{
    private readonly IEntryService _entryService;

    public EntryController(IEntryService entryService)
    {
        _entryService = entryService;
    }

    /// <summary>
    /// Links a song to a timeline by creating an entry.
    /// </summary>
    /// <param name="entryDto">The entry data containing timeline ID, song ID, and description.</param>
    /// <returns>
    /// 201 Created - If the entry is successfully created.
    /// 400 Bad Request - If an error occurs during creation.
    /// </returns>
    /// <example>
    /// POST: api/entry/LinkSongToTimeline
    /// Body:
    /// {
    ///   "timeline_Id": 1,
    ///   "SongId": 10,
    ///   "description": "Song added to timeline."
    /// }
    /// </example>
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

    /// <summary>
    /// Unlinks a song from a timeline by deleting the entry.
    /// </summary>
    /// <param name="timelineId">The ID of the timeline.</param>
    /// <param name="songId">The ID of the song.</param>
    /// <returns>
    /// 204 No Content - If the entry is successfully deleted.
    /// 400 Bad Request - If an error occurs.
    /// </returns>
    /// <example>
    /// DELETE: api/entry/UnlinkSongFromTimeline/1/10
    /// </example>
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

    /// <summary>
    /// Retrieves all entries for a specific timeline.
    /// </summary>
    /// <param name="timelineId">The ID of the timeline.</param>
    /// <returns>
    /// 200 OK
    /// [{EntryDto}, {EntryDto}, ...]
    /// </returns>
    /// <example>
    /// GET: api/entry/GetEntriesForTimeline/1
    /// </example>
    [HttpGet("GetEntriesForTimeline/{timelineId}")]
    public async Task<IActionResult> GetEntriesForTimeline(int timelineId)
    {
        var entries = await _entryService.GetEntriesForTimeline(timelineId);
        return Ok(entries);
    }

    /// <summary>
    /// Retrieves all entries for a specific song.
    /// </summary>
    /// <param name="songId">The ID of the song.</param>
    /// <returns>
    /// 200 OK
    /// [{EntryDto}, {EntryDto}, ...]
    /// </returns>
    /// <example>
    /// GET: api/entry/GetEntriesForSong/10
    /// </example>
    [HttpGet("GetEntriesForSong/{songId}")]
    public async Task<IActionResult> GetEntriesForSong(int songId)
    {
        var entries = await _entryService.GetEntriesForSong(songId);
        return Ok(entries);
    }

    /// <summary>
    /// Updates an existing entry.
    /// </summary>
    /// <param name="entryId">The ID of the entry to update.</param>
    /// <param name="updatedEntryDto">The updated entry data.</param>
    /// <returns>
    /// 204 No Content - If the update is successful.
    /// 400 Bad Request - If an error occurs during the update.
    /// </returns>
    /// <example>
    /// PUT: api/entry/UpdateEntry/5
    /// Body:
    /// {
    ///   "timeline_Id": 1,
    ///   "SongId": 10,
    ///   "description": "Updated description."
    /// }
    /// </example>
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

    /// <summary>
    /// Retrieves a specific entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the entry to retrieve.</param>
    /// <returns>
    /// 200 OK
    /// {EntryDto}
    /// 404 Not Found - If the entry does not exist.
    /// </returns>
    /// <example>
    /// GET: api/entry/find5
    /// </example>
    [HttpGet("find{id}")]
    public async Task<IActionResult> GetEntry(int id)
    {
        var award = await _entryService.FindEntry(id);
        if (award == null)
        {
            return NotFound(new { message = "Award not found" });
        }
        return Ok(award);
    }
}
