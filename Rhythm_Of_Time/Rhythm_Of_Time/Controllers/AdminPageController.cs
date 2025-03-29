using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Controllers
{
    public class AdminPageController : Controller
    {
        private readonly IUserServices _userService;
        private readonly ITimelineService _timelineService;
        private readonly IEntryService _entryService;
        private readonly IArtistService _artistService;
        private readonly ISongService _songService;
        private readonly IAwardService _awardService;
        private readonly IUserTimelineService _userTimelineService;
        private readonly IArtistSongService _artistSongService;
        private readonly IAwardSongService _awardSongService;

        public AdminPageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
        {
            _userService = userService;
            _timelineService = timelineService;
            _entryService = entryService;
            _artistService = artistService;
            _songService = songService;
            _awardService = awardService;
            _userTimelineService = userTimelineService;
            _artistSongService = artistSongService;
            _awardSongService = awardSongService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<UserDto?> userDtos = await _userService.List();
            return View(userDtos);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            UserDto? userDto = await _userService.FindUser(id);
            IEnumerable<UserTimelineDto> AssociatedTimelines = await _userTimelineService.GetTimelinesForUser(id);

            if (userDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find user"] });
            }
            else
            {
                UserDetails UserInfo = new UserDetails()
                {
                    User = userDto,
                    UserTimeline = AssociatedTimelines
                };
                return View(UserInfo);  // Pass UserDetails to the view
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            UserDto? userDto = await _userService.FindUser(id);
            if (userDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find user"] });
            }
            else
            {
                return View(userDto);
            }
        }

        // POST: UserPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(string id, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", userDto); // Return to form with errors
            }

            var response = await _userService.UpdateUser(id, userDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                // Optional: Sign user out globally
              
                return RedirectToAction("Details", "AdminPage", new { id });
            }

            // Handle errors
            foreach (var error in response.Messages)
            {
                ModelState.AddModelError("", error);
            }
            return View("Edit", userDto);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            UserDto? userDto = await _userService.FindUser(id);
            if (userDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find user"] });
            }
            else
            {
                return View(userDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            ServiceResponse response = await _userService.DeleteUser(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "AdminPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

    }
}
