using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rhythm_Of_Time.Services;

namespace Rhythm_Of_Time.Controllers
{
    public class AwardPageController : Controller
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

            public AwardPageController(IUserServices userService, ITimelineService timelineService, IEntryService entryService, IArtistService artistService, ISongService songService, IAwardService awardService, IUserTimelineService userTimelineService, IArtistSongService artistSongService, IAwardSongService awardSongService)
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


            // GET: Award/List
            public async Task<IActionResult> List()
            {
                // Retrieve a list of all awards from the service
                IEnumerable<AwardDto> awardDtos = await _awardService.List();
                return View(awardDtos);
            }

            // GET: Award/Details/{id}
            public async Task<IActionResult> Details(int id)
            {
                // Fetch the award by ID
                AwardDto? awardDto = await _awardService.FindAward(id);
                if (awardDto == null)
                {
                    return View("Error", new ErrorViewModel { Errors = new List<string> { "Could not find award." } });
                }

                // Fetch the songs that received this award
                IEnumerable<AwardSongDto> awardSongDtos = await _awardSongService.GetSongsForAward(id);
                List<SongDTO> associatedSongs = new List<SongDTO>();

                foreach (var awardSong in awardSongDtos)
                {
                    var song = await _songService.FindSong(awardSong.SongId);
                    if (song != null)
                    {
                        associatedSongs.Add(song);
                    }
                }

                // Fetch the artists of the awarded songs
                List<ArtistDto> associatedArtists = new List<ArtistDto>();
                foreach (var song in associatedSongs)
                {
                    var artistSongDtos = await _artistSongService.GetArtistsForSong(song.SongId);
                    foreach (var artistSong in artistSongDtos)
                    {
                        var artist = await _artistService.FindArtist(artistSong.ArtistId);
                        if (artist != null && !associatedArtists.Any(a => a.ArtistId == artist.ArtistId))
                        {
                            associatedArtists.Add(artist);
                        }
                    }
                }

                // Create ViewModel
                AwardDetails awardDetails = new AwardDetails()
                {
                    Award = awardDto,
                    Songs = associatedSongs,
                    Artists = associatedArtists
                };

                return View(awardDetails);
            }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AwardDto? awardDto = await _awardService.FindAward(id);
            if (awardDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find award"] });
            }
            return View(awardDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, AwardDto awardDto)
        {
            ServiceResponse response = await _awardService.UpdateAward(id, awardDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "AwardPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            AwardDto? awardDto = await _awardService.FindAward(id);
            if (awardDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find award"] });
            }
            return View(awardDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _awardService.DeleteAward(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "AwardPage");
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

        public async Task<IActionResult> Add(AwardDto awardDto)
        {
            ServiceResponse response = await _awardService.AddAward(awardDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "AwardPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

    }
}

