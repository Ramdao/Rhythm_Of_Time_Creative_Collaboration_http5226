
using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Data;
using Rhythm_Of_Time.Models;
using Microsoft.EntityFrameworkCore;
using Rhythm_Of_Time.Data.Migrations;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Services;
using Microsoft.AspNetCore.Authorization;


namespace Rhythm_Of_Time.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserServices _userService;

        // dependency injection of service interfaces
        public UserController(IUserServices context)
        {

            _userService = context;
        }
        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>
        /// 200 - OK: A list of all users.
        /// </returns>
        /// <example>
        /// GET: api/User/List
        /// </example>
        [HttpGet(template: "List")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> ListUsers()
        {
            return Ok(await _userService.List());
        }
        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDto">Updated user data.</param>
        /// <returns>
        /// 204 - No Content: If the user was successfully updated.<br/>
        /// 404 - Not Found: If the user does not exist.<br/>
        /// 500 - Internal Server Error: If an unexpected error occurs.
        /// </returns>
        /// <example>
        /// PUT: api/User/Update/123
        /// </example>

        [HttpPut(template: "Update/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutUser(string id, [FromBody] UserDto userDto)
        {
            var response = await _userService.UpdateUser(id, userDto);

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
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>
        /// 204 - No Content: If the user was successfully deleted.<br/>
        /// 404 - Not Found: If the user does not exist.<br/>
        /// 500 - Internal Server Error: If an unexpected error occurs.
        /// </returns>
        /// <example>
        /// DELETE: api/User/Delete/123
        /// </example>
        [HttpDelete(template: "Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto updatedUser)
        {
            var isUpdated = await _userService.UpdateUser(id, updatedUser);
            ServiceResponse response = await _userService.DeleteUser(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }

            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();
        }
        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="userDto">User data to be added.</param>
        /// <param name="password">Password for the new user.</param>
        /// <returns>
        /// 201 - Created: If the user was successfully added.<br/>
        /// 404 - Not Found: If the user cannot be added due to missing data.<br/>
        /// 500 - Internal Server Error: If an unexpected error occurs.
        /// </returns>
        /// <example>
        /// POST: api/User/Add
        /// </example>
        [HttpPost(template: "Add")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto, string password)
        {
            var response = await _userService.AddUser(userDto, password);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Created($"api/User/FindUser/{response.CreatedId}", userDto);
        }
        /// <summary>
        /// Retrieves a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to find.</param>
        /// <returns>
        /// 200 - OK: The user data if found.<br/>
        /// 404 - Not Found: If the user does not exist.
        /// </returns>
        /// <example>
        /// GET: api/User/Find/123
        /// </example>
        [HttpGet(template: "Find/{id}")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<UserDto>> FindUser(string id)
        {
            var user = await _userService.FindUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

    }
}