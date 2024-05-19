using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly PlayerSdk _playerSdk;
        public GameController(PlayerSdk playerSdk)
        {
            _playerSdk = playerSdk;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Leaderboard()
        {
            var players = await _playerSdk.Find();
            var sortedPlayers = players.OrderByDescending(p => p.Experience).ToList();
            return View("Leaderboard", sortedPlayers);
        }
    }
}
