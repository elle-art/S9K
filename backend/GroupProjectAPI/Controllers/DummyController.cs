using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services; // if FireBaseCommunications or DBCommunications is here
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DummyController : ControllerBase
    {
        [HttpPost("save")]
        public async Task<IActionResult> SaveDummy([FromQuery] string uid, [FromBody] Dummy dummy)
        {
            if (string.IsNullOrEmpty(uid) || dummy == null)
                return BadRequest("UID or Dummy is missing.");

            await DBCommunications.SaveDummyAsync(uid, dummy);
            return Ok(new { success = true });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetDummy([FromQuery] string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return BadRequest("UID is required.");

            Dummy dummy = await DBCommunications.GetDummyAsync(uid);

            if (dummy == null)
                return NotFound("Dummy not found.");

            return Ok(dummy);
        }
    }
}