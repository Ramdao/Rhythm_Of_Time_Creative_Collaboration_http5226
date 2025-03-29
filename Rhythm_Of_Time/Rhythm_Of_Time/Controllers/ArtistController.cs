using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        // GET: api/artist
        [HttpPost(template: "List")]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await _artistService.List();
            return Ok(artists);
        }

        // GET: api/artist/5
        [HttpGet("find{id}")]
        public async Task<IActionResult> GetArtist(int id)
        {
            var artist = await _artistService.FindArtist(id);
            if (artist == null)
            {
                return NotFound(new { message = "Artist not found" });
            }
            return Ok(artist);
        }

        // POST: api/artist
        [HttpPost(template: "add")]
        public async Task<IActionResult> CreateArtist([FromBody] ArtistDto artist)
        {
            if (artist == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var response = await _artistService.AddArtist(artist);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return CreatedAtAction(nameof(GetArtist), new { id = response.CreatedId }, artist);
            }

            return BadRequest(new { message = string.Join(", ", response.Messages) });
        }

        // PUT: api/artist/5
        [HttpPut("update{id}")]
        public async Task<IActionResult> UpdateArtist(int id, [FromBody] ArtistDto artist)
        {
            if (artist == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var response = await _artistService.UpdateArtist(id, artist);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "Artist not found" });
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = string.Join(", ", response.Messages) });
            }

            return NoContent(); // Indicate that the update was successful
        }

        // DELETE: api/artist/5
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var response = await _artistService.DeleteArtist(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "Artist not found" });
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = string.Join(", ", response.Messages) });
            }

            return NoContent(); // Indicate that the artist was deleted successfully
        }
    }
}
