using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Services;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Rhythm_Of_Time.Controllers
{
    public class LinkPageController : Controller
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

        public LinkPageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
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

        public async Task<IActionResult> UserTimeLineLink(string Id)
        {
            var users = await _userService.List();
            var timelines = await _timelineService.List();

            var viewModel = new UserTimeLineDetails
            {
                UserId = Id,
                Users = users,          
                Timelines = timelines  
            };

            return View(viewModel);  
        }

        public async Task<IActionResult> TimelineSongLink(string Id)
        {
            var songs = await _songService.List();
            var timelines = await _timelineService.List();

            var viewModel = new EntryLinkDetail
            {
                SongId = Id,
                Songs = songs,
                Timelines = timelines
            };
           

            return View(viewModel);
        }
        // POST: LinkPage/LinkUserToTimeline
        [HttpPost]
        public async Task<IActionResult> LinkUserToTimeline(string userId, int timelineId)
        {
            await _userTimelineService.LinkUserToTimeline(userId, timelineId);
            return RedirectToAction("UserTimeLineLink");
        }

        // POST: LinkPage/UnlinkUserFromTimeline
        [HttpPost]
        public async Task<IActionResult> UnlinkUserFromTimeline(string userId, int timelineId)
        {
            await _userTimelineService.UnlinkUserFromTimeline(userId, timelineId);
            return RedirectToAction("UserTimeLineLink");
        }

        // POST: Link a song to a timeline with a description
        [HttpPost]
        public async Task<IActionResult> LinkSongToTimeline(int songId, int timelineId, string description)
        {
            var response = await _entryService.LinkEntryToTimelineAndSong(timelineId, songId, description);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                TempData["Success"] = "Song successfully linked to timeline.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", response.Messages);
            }

            return RedirectToAction("UserTimeLineLink");
        }

        [HttpPost]
        public async Task<IActionResult> UnlinkSongFromTimeline(int songId, int timelineId)
        {
            var response = await _entryService.UnlinkEntry(timelineId, songId);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                TempData["Success"] = "Song successfully unlinked from timeline.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", response.Messages);
            }

            return RedirectToAction("UserTimeLineLink");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEntryDescription(int entryId, string newDescription)
        {
            var entryDto = new EntryDto { decription = newDescription };
            var response = await _entryService.UpdateEntry(entryId, entryDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                TempData["Success"] = "Entry description updated successfully.";
            }
            else
            {
                TempData["Error"] = string.Join(", ", response.Messages);
            }

            return RedirectToAction("UserTimeLineLink");
        }

        // GET: Fetch all entries for a timeline
        [HttpGet]
        public async Task<IActionResult> GetEntriesForTimeline(int timelineId)
        {
            var entries = await _entryService.GetEntriesForTimeline(timelineId);
            return Json(entries);
        }

        // GET: Fetch all entries for a song
        [HttpGet]
        public async Task<IActionResult> GetEntriesForSong(int songId)
        {
            var entries = await _entryService.GetEntriesForSong(songId);
            return Json(entries);
        }
    }
}
