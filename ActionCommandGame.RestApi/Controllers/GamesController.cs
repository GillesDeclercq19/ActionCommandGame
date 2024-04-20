using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("performAction")]
        public async Task<IActionResult> PerformAction(int playerId)
        {
            var result = await _gameService.PerformAction(playerId);
            return Ok(result);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy(int playerId, int itemId)
        {
            var result = await _gameService.Buy(playerId, itemId);
            return Ok(result);
        }
    }
}
