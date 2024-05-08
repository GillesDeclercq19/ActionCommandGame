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
    public class NegativeGameEventsController : ControllerBase
    {
        private readonly INegativeGameEventService _negativeGameEventService;
        public NegativeGameEventsController(INegativeGameEventService negativeGameEventService)
        {
            _negativeGameEventService = negativeGameEventService;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _negativeGameEventService.Find();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _negativeGameEventService.Get(id);
            return Ok(result);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomNegativeGameEvent()
        {
            var result = await _negativeGameEventService.GetRandomNegativeGameEvent();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NegativeGameEventRequest negativeEvent)
        {
            var result = await _negativeGameEventService.Create(negativeEvent);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, NegativeGameEventRequest negativeEvent)
        {
            var result = await _negativeGameEventService.Update(id, negativeEvent);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _negativeGameEventService.Delete(id);
            return Ok();
        }
    }
}
