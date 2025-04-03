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

        /// <summary>
        /// Retrieves a list of all songs.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{SongDTO}, {SongDTO}, ...]
        /// </returns>
        /// <example>
        /// POST: api/song/List
        /// </example>
        [HttpPost(template: "List")]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _songService.List();
            return Ok(songs);
        }

        /// <summary>
        /// Retrieves a specific song by its ID.
        /// </summary>
        /// <param name="id">The ID of the song to retrieve.</param>
        /// <returns>
        /// 200 OK
        /// {SongDTO}
        /// 404 Not Found - If the song does not exist.
        /// </returns>
        /// <example>
        /// GET: api/song/5
        /// </example>
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

        /// <summary>
        /// Creates a new song.
        /// </summary>
        /// <param name="songDto">The song data transfer object.</param>
        /// <returns>
        /// 201 Created - If the song is successfully created.
        /// 400 Bad Request - If the provided data is invalid.
        /// </returns>
        /// <example>
        /// POST: api/song/Add
        /// Body:
        /// {
        ///   "title": "Song Title",
        ///   "artist": "Artist Name",
        ///   "duration": "3:45",
        ///   ...
        /// }
        /// </example
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

        /// <summary>
        /// Updates an existing song.
        /// </summary>
        /// <param name="id">The ID of the song to update.</param>
        /// <param name="songDto">The updated song data.</param>
        /// <returns>
        /// 204 No Content - If the update is successful.
        /// 404 Not Found - If the song does not exist.
        /// 400 Bad Request - If the provided data is invalid.
        /// </returns>
        /// <example>
        /// PUT: api/song/Update5
        /// Body:
        /// {
        ///   "title": "Updated Title",
        ///   "artist": "Updated Artist",
        ///   "duration": "4:00",
        ///   ...
        /// }
        /// </example>
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

        /// <summary>
        /// Deletes a song by ID.
        /// </summary>
        /// <param name="id">The ID of the song to delete.</param>
        /// <returns>
        /// 204 No Content - If the song is successfully deleted.
        /// 404 Not Found - If the song does not exist.
        /// 400 Bad Request - If an error occurs during deletion.
        /// </returns>
        /// <example>
        /// DELETE: api/song/5
        /// </example>
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
