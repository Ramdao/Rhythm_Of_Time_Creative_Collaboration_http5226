using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Controllers
{
    public class TimelinePageController : Controller
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

        public TimelinePageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
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
                IEnumerable<TimelineDto> timelineDtos = await _timelineService.List();
                return View(timelineDtos);
            }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the timeline by ID
            TimelineDto? timelineDto = await _timelineService.FindTimeline(id);

            // Fetch the users associated with this timeline
            IEnumerable<UserTimelineDto> associatedUsers = await _userTimelineService.GetUsersForTimeline(id);

            // Fetch the entries associated with this timeline
            IEnumerable<EntryDto> associatedEntries = await _entryService.GetEntriesForTimeline(id);

            // Check if the timeline exists
            if (timelineDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find timeline"] });
            }
            else
            {
                // Prepare the TimelineDetails view model
                TimelineDetails timelineInfo = new TimelineDetails()
                {
                    Timeline = timelineDto,
                    UserTimeline = associatedUsers,
                    Entries = associatedEntries
                };

                return View(timelineInfo);  // Pass TimelineDetails to the view
            }
        }
        // POST: TimelinePage/Add
        public IActionResult New()
        {
            return View();
        }

        public async Task<IActionResult> Add(TimelineDto timelineDto)
        {
            ServiceResponse response = await _timelineService.AddTimeline(timelineDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "TimelinePage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET: TimelinePage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            TimelineDto? timelineDto = await _timelineService.FindTimeline(id);
            if (timelineDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find timeline"] });
            }
            return View(timelineDto);  
        }

        // POST: TimelinePage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, TimelineDto timelineDto)
        {
            ServiceResponse response = await _timelineService.UpdateTimeline(id, timelineDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "TimelinePage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
        // GET: TimelinePage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            TimelineDto? timelineDto = await _timelineService.FindTimeline(id);
            if (timelineDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find timeline"] });
            }
            return View(timelineDto);  // Pass timelineDto to the view
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _timelineService.DeleteTimeline(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "TimelinePage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

    }

       
    }


