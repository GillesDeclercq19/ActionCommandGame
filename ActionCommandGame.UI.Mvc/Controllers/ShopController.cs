using ActionCommandGame.Sdk;
using ActionCommandGame.UI.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.UI.Mvc.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly ItemSdk _itemSdk;
        private readonly GameSdk _gameSdk;

        public ShopController(ItemSdk itemSdk, GameSdk gameSdk)
        {
            _itemSdk = itemSdk;
            _gameSdk = gameSdk;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemSdk.Find();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int itemId)
        {
            var playerId = int.Parse(User.Claims.FirstOrDefault(o => o.Type == "PlayerId")?.Value ?? "0");
            var result = await _gameSdk.Buy(playerId, itemId);

            return RedirectToAction("Index", "Shop");
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int playerId, int itemId)
        {
            var item = await _itemSdk.Get(itemId);

            var name = item.Name;
            ViewBag.Name = name;

            var bought = new BuyModel
            {
                PlayerId = playerId,
                ItemId = itemId
            };
            return View(bought);
        }
    }
}
