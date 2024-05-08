using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PositiveGameEventsController : ControllerBase
    {
        private readonly IPositiveGameEventService _positiveGameEventService;
        public PositiveGameEventsController(IPositiveGameEventService positiveGameEventService)
        {
            _positiveGameEventService = positiveGameEventService;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _positiveGameEventService.Find();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _positiveGameEventService.Get(id);
            return Ok(result);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPositiveGameEvent([FromQuery] bool hasAttackItem)
        {
            var result = await _positiveGameEventService.GetRandomPositiveGameEvent(hasAttackItem);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PositiveGameEventRequest positiveEvent)
        {
            var result = await _positiveGameEventService.Create(positiveEvent);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PositiveGameEventRequest positiveEvent)
        {
            var result = await _positiveGameEventService.Update(id, positiveEvent);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _positiveGameEventService.Delete(id);
            return Ok();
        }
    }

}

