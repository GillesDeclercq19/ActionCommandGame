using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly PlayerSdk _playerSdk;
        private readonly GameSdk _gameSdk;
        private readonly PlayerItemSdk _playerItemSdk;

        public GameController(PlayerSdk playerSdk, GameSdk gameSdk, PlayerItemSdk playerItemSdk)
        {
            _playerSdk = playerSdk;
            _gameSdk = gameSdk;
            _playerItemSdk = playerItemSdk;
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

        [HttpPost]
        public async Task<IActionResult> PerformAction()
        {
            var playerId = int.Parse(User.Claims.FirstOrDefault(o => o.Type == "PlayerId")?.Value ?? "0");
            var result = await _gameSdk.PerformAction(playerId);

            if (result.IsSuccessful && result.Data != null)
            {
                var playerInfo = await _playerSdk.Get(playerId) ?? new PlayerResult();
                var playerItems = await _playerItemSdk.Find(playerId);

                var playModel = new PlayModel
                {
                    GameResults = result.Data,
                    PlayerResult = playerInfo
                };
                playModel.GameResults.EventMessages = result.Messages;

                ViewData["Attack"] = playerItems.Sum(item => item.RemainingAttack);
                ViewData["Ki"] = playerItems.Sum(item => item.RemainingKi);
                ViewData["Defence"] = playerItems.Sum(item => item.RemainingDefense);

                return View("Index", playModel);
            }

            return RedirectToAction("Error", "Home");
        }
    }
}