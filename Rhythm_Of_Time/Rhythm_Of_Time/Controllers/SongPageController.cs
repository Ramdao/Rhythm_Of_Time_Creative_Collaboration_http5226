using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Controllers
{
    public class SongPageController : Controller
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

        public SongPageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
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
            var songDto = await _songService.FindSong(id);
            var associatedArtists = await _artistSongService.GetArtistsForSong(id);
            var associatedAwards = await _awardSongService.GetAwardsForSong(id);

            

            if (songDto == null) return View("Error", new ErrorViewModel { Errors = ["Could not find song"] });

            return View(new SongDetails
            {
                Song = songDto,
                Artists = associatedArtists,
                Awards = associatedAwards
            });
        }



        public async Task<IActionResult> List()
        {
            IEnumerable<SongDTO> songDtos = await _songService.List();
            return View(songDtos);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            SongDTO? songDto = await _songService.FindSong(id);
            if (songDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find Song"] });
            }
            return View(songDto);
        }

        // POST: TimelinePage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, SongDTO songDto)
        {
            ServiceResponse response = await _songService.UpdateSong(id, songDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "SongPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET: SongPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            SongDTO? SongDto = await _songService.FindSong(id);
            if (SongDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find song"] });
            }
            return View(SongDto);  // Pass timelineDto to the view
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _songService.DeleteSong(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "SongPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // POST: SongPage/Add
        public IActionResult New()
        {
            return View();
        }

        public async Task<IActionResult> Add(SongDTO SongDto)
        {
            ServiceResponse response = await _songService.AddSong(SongDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "SongPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
