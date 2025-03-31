using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Controllers
{
    public class EntryPageController : Controller
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

        public EntryPageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
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
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the entry by ID
            EntryDto? entryDto = await _entryService.FindEntry(id);
            if (entryDto == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Could not find entry." } });
            }

            // Fetch the timeline associated with this entry
            TimelineDto? timelineDto = await _timelineService.FindTimeline(entryDto.timeline_Id);

            // Fetch the entries associated with this timeline (this entry will be included)
            IEnumerable<EntryDto> associatedEntries = await _entryService.GetEntriesForTimeline(entryDto.timeline_Id);

            // Fetch the songs associated with this timeline
            SongDTO? associatedSongs = await _songService.FindSong(entryDto.SongId);

            // Check if the timeline exists
            if (timelineDto == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Could not find associated timeline." } });
            }

            // Prepare the EntryDetails view model
            var entryDetails = new EntryDetails
            {
                Entry = entryDto,
                Timeline = timelineDto,
                AssociatedEntries = associatedEntries,
                Song = associatedSongs 
            };

            return View(entryDetails);
        }
    }
}
