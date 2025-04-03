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

        /// <summary>
        /// Retrieves a list of all artists.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{ArtistDto}, {ArtistDto}, ...]
        /// </returns>
        /// <example>
        /// POST: api/artist/List
        /// </example>
        [HttpPost(template: "List")]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await _artistService.List();
            return Ok(artists);
        }

        /// <summary>
        /// Retrieves a specific artist by their ID.
        /// </summary>
        /// <param name="id">The ID of the artist to retrieve.</param>
        /// <returns>
        /// 200 OK
        /// {ArtistDto}
        /// 404 Not Found - If the artist does not exist.
        /// </returns>
        /// <example>
        /// GET: api/artist/find5
        /// </example>
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

        /// <summary>
        /// Creates a new artist.
        /// </summary>
        /// <param name="artist">The artist data transfer object.</param>
        /// <returns>
        /// 201 Created - If the artist is successfully created.
        /// 400 Bad Request - If the provided data is invalid.
        /// </returns>
        /// <example>
        /// POST: api/artist/add
        /// Body:
        /// {
        ///   "name": "Artist Name",
        ///   "genre": "Genre",
        ///   ...
        /// }
        /// </example>
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

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        /// <param name="id">The ID of the artist to update.</param>
        /// <param name="artist">The updated artist data.</param>
        /// <returns>
        /// 204 No Content - If the update is successful.
        /// 404 Not Found - If the artist does not exist.
        /// 400 Bad Request - If the provided data is invalid.
        /// </returns>
        /// <example>
        /// PUT: api/artist/update5
        /// Body:
        /// {
        ///   "name": "Updated Artist Name",
        ///   "genre": "Updated Genre",
        ///   ...
        /// }
        /// </example>
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

        /// <summary>
        /// Deletes an artist by ID.
        /// </summary>
        /// <param name="id">The ID of the artist to delete.</param>
        /// <returns>
        /// 204 No Content - If the artist is successfully deleted.
        /// 404 Not Found - If the artist does not exist.
        /// 400 Bad Request - If an error occurs during deletion.
        /// </returns>
        /// <example>
        /// DELETE: api/artist/delete5
        /// </example>
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
