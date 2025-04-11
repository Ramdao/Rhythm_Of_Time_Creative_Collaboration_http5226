using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using System.Security.Claims;

namespace Rhythm_Of_Time.Controllers
{
    public class UserPageController : Controller
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

        public UserPageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
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
        public async Task<IActionResult> User(string id)
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

    }
}

   
