
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
    }
}