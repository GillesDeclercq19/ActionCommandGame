using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.UI.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PlayersController : Controller
    {
        private readonly PlayerSdk _playerSdk;

        public PlayersController(PlayerSdk playerSdk)
        {
            _playerSdk = playerSdk;
        }

        public async Task<IActionResult> Index()
        {
            var players = await _playerSdk.Find();
            return View(players);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerRequest player)
        {
            if (!ModelState.IsValid)
            {
                return View(player);
            }

            var userId = User.Claims.Where(o => o.Type == "Id").Select(o => o.Value).First();
            player.UserId = userId;
            await _playerSdk.Create(player);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var player = await _playerSdk.Get(id);

            if (player is null)
            {
                return RedirectToAction("Index");
            }

            var playerRequest = new PlayerRequest()
            {
                Name = player.Name,
                Zeni = player.Zeni,
                Experience = player.Experience,
                UserId = player.UserId
            };

            return View(playerRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, PlayerRequest player)
        {
            if (!ModelState.IsValid)
            {
                return View(player);
            }

            await _playerSdk.Update(id, player);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var player = await _playerSdk.Get(id);
            return View(player);
        }

        [HttpPost("Admin/players/delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _playerSdk.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
