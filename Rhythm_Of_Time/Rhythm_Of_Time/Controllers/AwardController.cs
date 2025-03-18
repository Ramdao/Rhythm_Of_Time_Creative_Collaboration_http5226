using Microsoft.AspNetCore.Mvc;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardController : ControllerBase
    {
        private readonly IAwardService _awardService;

        public AwardController(IAwardService awardService)
        {
            _awardService = awardService;
        }

        // GET: api/award
        [HttpPost(template: "List")]
        public async Task<IActionResult> GetAwards()
        {
            var awards = await _awardService.List();
            return Ok(awards);
        }

        // GET: api/award/5
        [HttpGet("find{id}")]
        public async Task<IActionResult> GetAward(int id)
        {
            var award = await _awardService.FindAward(id);
            if (award == null)
            {
                return NotFound(new { message = "Award not found" });
            }
            return Ok(award);
        }

        // POST: api/award
        [HttpPost(template: "add")]
        public async Task<IActionResult> CreateAward([FromBody] AwardDto awardDto)
        {
            if (awardDto == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var response = await _awardService.AddAward(awardDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return CreatedAtAction(nameof(GetAward), new { id = response.CreatedId }, awardDto);
            }

            return BadRequest(new { message = string.Join(", ", response.Messages) });
        }

        // PUT: api/award/5
        [HttpPut("update{id}")]
        public async Task<IActionResult> UpdateAward(int id, [FromBody] AwardDto awardDto)
        {
            if (awardDto == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var response = await _awardService.UpdateAward(id, awardDto);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "Award not found" });
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = string.Join(", ", response.Messages) });
            }

            return NoContent();
        }

        // DELETE: api/award/5
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> DeleteAward(int id)
        {
            var response = await _awardService.DeleteAward(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "Award not found" });
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = string.Join(", ", response.Messages) });
            }

            return NoContent();
        }
    }
}
