
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
        [HttpGet(template: "List")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> ListUsers()
        {
            return Ok(await _userService.List());
        }

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