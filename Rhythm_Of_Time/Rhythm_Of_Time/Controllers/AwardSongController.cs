using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AwardSongController : ControllerBase
    {
        private readonly IAwardSongService _awardSongService;

        public AwardSongController(IAwardSongService awardSongService)
        {
            _awardSongService = awardSongService;
        }

        // Link an Award to a Song
        [HttpPost("LinkAwardToSong")]
        public async Task<IActionResult> LinkAwardToSong([FromBody] AwardSongDto awardSongDto)
        {
            var response = await _awardSongService.LinkAwardToSong(awardSongDto.SongId, awardSongDto.AwardId, awardSongDto.Status);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return CreatedAtAction(nameof(GetSongsForAward), new { awardId = awardSongDto.AwardId }, awardSongDto);
            }

            return BadRequest(response);
        }

        // Unlink an Award from a Song
        [HttpDelete("UnlinkAwardFromSong/{songId}/{awardId}")]
        public async Task<IActionResult> UnlinkAwardFromSong(int songId, int awardId)
        {
            var response = await _awardSongService.UnlinkAwardFromSong(songId, awardId);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return NoContent(); // Successfully deleted
            }

            return BadRequest(response);
        }

        // Get all Awards linked to a Song
        [HttpGet("GetAwardsForSong/{songId}")]
        public async Task<IActionResult> GetAwardsForSong(int songId)
        {
            var awards = await _awardSongService.GetAwardsForSong(songId);
            return Ok(awards);
        }

        // Get all Songs linked to an Award
        [HttpGet("GetSongsForAward/{awardId}")]
        public async Task<IActionResult> GetSongsForAward(int awardId)
        {
            var songs = await _awardSongService.GetSongsForAward(awardId);
            return Ok(songs);
        }
    }
}
