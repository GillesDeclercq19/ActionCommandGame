using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.UI.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.UI.Mvc.Controllers
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

            return RedirectToAction("Play");
        }

        public IActionResult Guide()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var playerId = int.Parse(User.Claims.FirstOrDefault(o => o.Type == "PlayerId")?.Value ?? "0");
            var playerInfo = await _playerSdk.Get(playerId);

            if (playerInfo == null)
            {
                return RedirectToAction("Index");
            }

            var playerRequest = new PlayerRequest
            {
                Name = playerInfo.Name,
                Zeni = playerInfo.Zeni,
                Experience = playerInfo.Experience,
                UserId = playerInfo.UserId
            };

            return View(playerRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerRequest player)
        {
            if (!ModelState.IsValid)
            {
                return View(player);
            }

            var playerId = int.Parse(User.Claims.FirstOrDefault(o => o.Type == "PlayerId")?.Value ?? "0");
            await _playerSdk.Update(playerId, player);

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Inventory()
        {
            var playerId = int.Parse(User.Claims.FirstOrDefault(o => o.Type == "PlayerId")?.Value ?? "0");
            var playerInfo = await _playerSdk.Get(playerId) ?? new PlayerResult();

            return View("Inventory", playerInfo);
        }

        public async Task<IActionResult> Leaderboard()
        {
            var players = await _playerSdk.Find();
            var sortedPlayers = players.OrderByDescending(p => p.Experience).ToList();
            return View("Leaderboard", sortedPlayers);
        }

        public async Task<IActionResult> Play(PlayModel? playModel)
        {
            var playerId = int.Parse(User.Claims.FirstOrDefault(o => o.Type == "PlayerId")?.Value ?? "0");
            var playerInfo = await _playerSdk.Get(playerId) ?? new PlayerResult();

            PlayModel info = new PlayModel
            {
                PlayerResult = playerInfo
            };

            var playerItems = await _playerItemSdk.Find(playerInfo.Id);

            var attack = playerItems.Sum(item => item.RemainingAttack);
            var ki = playerItems.Sum(item => item.RemainingKi);
            var defense = playerItems.Sum(item => item.RemainingDefense);

            ViewData["Attack"] = attack;
            ViewData["Ki"] = ki;
            ViewData["Defense"] = defense;

            return View("Index", info);
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
                ViewData["Defense"] = playerItems.Sum(item => item.RemainingDefense);

                return View("Index", playModel);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}