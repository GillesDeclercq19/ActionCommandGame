using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.UI.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ItemsController : Controller
    {
        private readonly ItemSdk _itemSdk;

        public ItemsController(ItemSdk itemSdk)
        {
            _itemSdk = itemSdk;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemSdk.Find();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemRequest item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            await _itemSdk.Create(item);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var item = await _itemSdk.Get(id);

            if (item is null)
            {
                return RedirectToAction("Index");
            }

            var itemRequest = new ItemRequest()
            {
                Name = item.Name,
                Description = item.Description,
                ActionCooldownSeconds = item.ActionCooldownSeconds,
                Attack = item.Attack,
                Defense = item.Defense,
                Ki = item.Ki,
                Price = item.Price
            };

            return View(itemRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, ItemRequest item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            await _itemSdk.Update(id, item);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var item = await _itemSdk.Get(id);
            return View(item);
        }

        [HttpPost("Admin/items/delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _itemSdk.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
