using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistSongController : ControllerBase
    {
        private readonly IArtistSongService _artistSongService;

        public ArtistSongController(IArtistSongService artistSongService)
        {
            _artistSongService = artistSongService;
        }

        // Link an Artist to a Song
        [HttpPost("LinkArtistToSong")]
        public async Task<IActionResult> LinkArtistToSong([FromBody] ArtistSongDto artistSongDto)
        {
            var response = await _artistSongService.LinkArtistToSong(artistSongDto.SongId, artistSongDto.ArtistId, artistSongDto.role);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return CreatedAtAction(nameof(GetSongsForArtist), new { artistId = artistSongDto.ArtistId }, artistSongDto);
            }

            return BadRequest(response);
        }

        // Unlink an Artist from a Song
        [HttpDelete("UnlinkArtistFromSong/{songId}/{artistId}")]
        public async Task<IActionResult> UnlinkArtistFromSong(int songId, int artistId)
        {
            var response = await _artistSongService.UnlinkArtistFromSong(songId, artistId);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return NoContent(); // Successfully deleted
            }

            return BadRequest(response);
        }

        // Get all Artists linked to a Song
        [HttpGet("GetArtistsForSong/{songId}")]
        public async Task<IActionResult> GetArtistsForSong(int songId)
        {
            var artists = await _artistSongService.GetArtistsForSong(songId);
            return Ok(artists);
        }

        // Get all Songs linked to an Artist
        [HttpGet("GetSongsForArtist/{artistId}")]
        public async Task<IActionResult> GetSongsForArtist(int artistId)
        {
            var songs = await _artistSongService.GetSongsForArtist(artistId);
            return Ok(songs);
        }
    }
}
