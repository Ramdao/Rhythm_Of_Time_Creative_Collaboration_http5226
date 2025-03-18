using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        // GET: api/song
        [HttpPost(template: "List")]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _songService.List();
            return Ok(songs);
        }

        // GET: api/song/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(int id)
        {
            var song = await _songService.FindSong(id);
            if (song == null)
            {
                return NotFound(new { message = "Song not found" });
            }
            return Ok(song);
        }

        // POST: api/song
        [HttpPost(template: "Add")]
        public async Task<IActionResult> CreateSong([FromBody] SongDTO songDto)
        {
            if (songDto == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var response = await _songService.AddSong(songDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return CreatedAtAction(nameof(GetSong), new { id = response.CreatedId }, songDto);
            }

            return BadRequest(new { message = string.Join(", ", response.Messages) });
        }

        // PUT: api/song/5
        [HttpPut("Update{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] SongDTO songDto)
        {
            if (songDto == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var response = await _songService.UpdateSong(id, songDto);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "Song not found" });
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = string.Join(", ", response.Messages) });
            }

            return NoContent(); // Indicate that the update was successful
        }

        // DELETE: api/song/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var response = await _songService.DeleteSong(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "Song not found" });
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = string.Join(", ", response.Messages) });
            }

            return NoContent(); // Indicate that the song was deleted successfully
        }
    }
}
