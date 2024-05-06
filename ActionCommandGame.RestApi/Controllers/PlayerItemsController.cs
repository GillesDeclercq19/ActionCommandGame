using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayerItemsController : ControllerBase
    {
        private readonly IPlayerItemService _playerItemService;

        public PlayerItemsController(IPlayerItemService playerItemService)
        {
            _playerItemService = playerItemService;
        }

        [HttpGet]
        public async Task<IActionResult> Find(int? playerId = null)
        {
            var playerItems = await _playerItemService.Find(playerId);
            return Ok(playerItems);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _playerItemService.Get(id);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _playerItemService.Delete(id);
            return Ok();
        }
    }
}
