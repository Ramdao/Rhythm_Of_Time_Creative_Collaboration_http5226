using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Rhythm_Of_Time.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTimelineController : Controller
    {

        private readonly IUserTimelineService _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserTimelineController(IUserTimelineService context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Links a user to a timeline.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="timelineId">The ID of the timeline.</param>
        /// <returns>
        /// 204 No Content - If the user is successfully linked to the timeline.
        /// 404 Not Found - If either the user or timeline does not exist.
        /// 409 Conflict - If the user is already linked to the timeline.
        /// 500 Internal Server Error - If an error occurs while processing.
        /// </returns>
        [HttpPost("LinkUserToTimeline")]
        public async Task<ActionResult> LinkUserToTimeline(string userId, int timelineId)
        {
            // Check if the user exists in Identity
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            ServiceResponse response = await _context.LinkUserToTimeline(userId, timelineId);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return Conflict(response.Messages);
            }

            return NoContent();
        }

        /// <summary>
        /// Unlinks a user from a timeline.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="timelineId">The ID of the timeline.</param>
        /// <returns>
        /// 204 No Content - If the user is successfully unlinked from the timeline.
        /// 404 Not Found - If the user is not linked to the specified timeline.
        /// 500 Internal Server Error - If an error occurs while processing.
        /// </returns>
        [HttpDelete("UnlinkUserFromTimeline")]
        public async Task<ActionResult> UnlinkUserFromTimeline(string userId, int timelineId)
        {
            // Check if the user exists in Identity
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            ServiceResponse response = await _context.UnlinkUserFromTimeline(userId, timelineId);

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
        /// Gets all timelines linked to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>
        /// 200 OK - A list of timelines linked to the user.
        /// 404 Not Found - If the user does not exist.
        /// 500 Internal Server Error - If an error occurs while processing.
        /// </returns>
        /// <example>
        /// GET: api/UserTimeline/GetTimelinesForUser/{userId} -> [1, 2, 3]
        /// </example>
        [HttpGet("GetTimelinesForUser/{userId}")]
        public async Task<ActionResult<IEnumerable<UserTimelineDto>>> GetTimelinesForUser(string userId)
        {
            // Check if the user exists in Identity
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            IEnumerable<UserTimelineDto> response = await _context.GetTimelinesForUser(userId);

            if (!response.Any())
            {
                return NotFound("No timelines associated with the user.");
            }

            return Ok(response);
        }

        /// <summary>
        /// Retrieves all users linked to a specific timeline.
        /// </summary>
        /// <param name="timelineId">The ID of the timeline.</param>
        /// <returns>
        /// 200 OK - A list of users linked to the timeline.
        /// 404 Not Found - If the timeline does not exist.
        /// 500 Internal Server Error - If an error occurs while processing.
        /// </returns>
        /// <example>
        /// GET : api/UserTimeline/GetUsersForTimeline/{timelineId} -> [userId1, userId2]
        /// </example>
        [HttpGet("GetUsersForTimeline/{timelineId}")]
        public async Task<ActionResult<IEnumerable<UserTimelineDto>>> GetUsersForTimeline(int timelineId)
        {
            IEnumerable<UserTimelineDto> response = await _context.GetUsersForTimeline(timelineId);

            if (!response.Any())
            {
                return NotFound("Timeline not found or no users associated.");
            }

            return Ok(response);
        }
    }
}
