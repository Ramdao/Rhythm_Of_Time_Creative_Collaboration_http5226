using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class TimelineController : Controller
        {
            private readonly ITimelineService _context;

            // Dependency injection of service interface
            public TimelineController(ITimelineService context)
            {
                _context = context;
            }

            /// <summary>
            /// Returns a list of all timelines.
            /// </summary>
            /// <returns>
            /// 200 OK
            /// [{TimelineDto}, {TimelineDto}, ...]
            /// </returns>
            /// <example>
            /// GET: api/Timeline/ListTimeline
            /// </example>
            [HttpGet("ListTimeline")]
            public async Task<ActionResult<IEnumerable<TimelineDto>>> GetTimelines()
            {
                IEnumerable<TimelineDto> timelineDtos = await _context.List();
                return Ok(timelineDtos);
            }

            /// <summary>
            /// Returns a single timeline by {id}.
            /// </summary>
            /// <param name="id">The timeline ID</param>
            /// <returns>
            /// 200 OK
            /// {TimelineDto}
            /// or
            /// 404 Not Found
            /// </returns>
            /// <example>
            /// GET: api/Timeline/Find/1
            /// </example>
            [HttpGet("Find/{id}")]
            public async Task<ActionResult<TimelineDto>> FindTimeline(int id)
            {
                var timeline = await _context.FindTimeline(id);
                if (timeline == null)
                {
                    return NotFound();
                }
                return Ok(timeline);
            }

            /// <summary>
            /// Updates an existing timeline.
            /// </summary>
            /// <param name="id">ID of the timeline to update</param>
            /// <param name="timelineDto">Updated timeline data</param>
            /// <returns>
            /// 400 Bad Request
            /// or
            /// 404 Not Found
            /// or
            /// 204 No Content
            /// </returns>
            /// <example>
            /// PUT: api/Timeline/Update/3
            /// </example>
            [HttpPut("Update/{id}")]
            public async Task<IActionResult> UpdateTimeline(int id, TimelineDto timelineDto)
            {
                var response = await _context.UpdateTimeline(id, timelineDto);
                if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                {
                    return NotFound(response.Messages);
                }
                else if (response.Status == ServiceResponse.ServiceStatus.Error)
                {
                    return StatusCode(500, response.Messages);
                }
                return NoContent();
            }

            /// <summary>
            /// Adds a new timeline entry.
            /// </summary>
            /// <param name="timelineDto">Timeline data</param>
            /// <returns>
            /// 201 Created
            /// or
            /// 404 Not Found
            /// </returns>
            /// <example>
            /// POST: api/Timeline/Add
            /// </example>
            [HttpPost("Add")]
            public async Task<ActionResult<Timeline>> AddTimeline(TimelineDto timelineDto)
            {
                ServiceResponse response = await _context.AddTimeline(timelineDto);
                if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                {
                    return NotFound(response.Messages);
                }
                else if (response.Status == ServiceResponse.ServiceStatus.Error)
                {
                    return StatusCode(500, response.Messages);
                }
                return Created($"api/Timeline/Find/{response.CreatedId}", timelineDto);
            }

            /// <summary>
            /// Deletes a timeline entry by {id}.
            /// </summary>
            /// <param name="id">ID of the timeline to delete</param>
            /// <returns>
            /// 204 No Content
            /// or
            /// 404 Not Found
            /// </returns>
            /// <example>
            /// DELETE: api/Timeline/Delete/4
            /// </example>
            [HttpDelete("Delete/{id}")]
            public async Task<IActionResult> DeleteTimeline(int id)
            {
                ServiceResponse response = await _context.DeleteTimeline(id);
                if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                {
                    return NotFound(response.Messages);
                }
                else if (response.Status == ServiceResponse.ServiceStatus.Error)
                {
                    return StatusCode(500, response.Messages);
                }
                return NoContent();
            }
        }
    }

