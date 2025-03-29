using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Controllers
{
    public class ArtistPageController : Controller
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

        public ArtistPageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
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

        // GET: Artist/List
        public async Task<IActionResult> List()
        {
            // Retrieve all artists
            IEnumerable<ArtistDto> artistDtos = await _artistService.List();
            return View(artistDtos);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the artist by ID
            ArtistDto? artist = await _artistService.FindArtist(id);
            if (artist == null)
            {
                return View("Error", new ErrorViewModel { Errors = ["Could not find artist"] });
            }

            // Fetch associated songs for the artist
            IEnumerable<ArtistSongDto> artistSongDtos = await _artistSongService.GetSongsForArtist(id);
            List<SongDTO> associatedSongs = new List<SongDTO>();

            foreach (var artistSong in artistSongDtos)
            {
                var song = await _songService.FindSong(artistSong.SongId);
                if (song != null)
                {
                    associatedSongs.Add(song);
                }
            }

            // Fetch associated awards for the artist
            IEnumerable<AwardSongDto> awardSongDtos = await _awardSongService.GetSongsForAward(id);
            List<AwardDto> associatedAwards = new List<AwardDto>();

            foreach (var awardSong in awardSongDtos)
            {
                var award = await _awardService.FindAward(awardSong.AwardId);
                if (award != null)
                {
                    associatedAwards.Add(award);
                }
            }

            // Use ArtistDetails ViewModel
            ArtistDetails artistDetails = new ArtistDetails()
            {
                Artist = artist,
                Song = associatedSongs,
                Awards = associatedAwards
            };

            return View(artistDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ArtistDto? artistDto = await _artistService.FindArtist(id);
            if (artistDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find artist"] });
            }
            return View(artistDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ArtistDto artistDto)
        {
            ServiceResponse response = await _artistService.UpdateArtist(id, artistDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "ArtistPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            ArtistDto? artistDto = await _artistService.FindArtist(id);
            if (artistDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find artist"] });
            }
            return View(artistDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _artistService.DeleteArtist(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ArtistPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        public IActionResult New()
        {
            return View();
        }

        public async Task<IActionResult> Add(ArtistDto artistDto)
        {
            ServiceResponse response = await _artistService.AddArtist(artistDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "ArtistPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
