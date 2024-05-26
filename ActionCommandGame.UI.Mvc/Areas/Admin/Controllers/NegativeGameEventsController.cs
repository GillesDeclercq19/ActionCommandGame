using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.UI.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NegativeGameEventsController : Controller
    {
        private readonly NegativeGameEventSdk _negativeGameEventSdk;
        public NegativeGameEventsController(NegativeGameEventSdk negativeGameEventSdk)
        {
            _negativeGameEventSdk = negativeGameEventSdk;
        }

        public async Task<IActionResult> Index()
        {
            var negativeGameEvent = await _negativeGameEventSdk.Find();
            return View(negativeGameEvent);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NegativeGameEventRequest negativeGameEvent)
        {
            if (!ModelState.IsValid)
            {
                return View(negativeGameEvent);
            }

            await _negativeGameEventSdk.Create(negativeGameEvent);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var negativeGameEvent = await _negativeGameEventSdk.Get(id);

            if (negativeGameEvent is null)
            {
                return RedirectToAction("Index");
            }

            var negativeGameEventRequest = new NegativeGameEventRequest()
            {
                Name = negativeGameEvent.Name,
                Description = negativeGameEvent.Description,
                DefenseWithGearDescription = negativeGameEvent.DefenseWithGearDescription,
                DefenseWithoutGearDescription = negativeGameEvent.DefenseWithoutGearDescription,
                DefenseLoss = negativeGameEvent.DefenseLoss,
                Probability = negativeGameEvent.Probability,
            };

            return View(negativeGameEventRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, NegativeGameEventRequest negativeGameEvent)
        {
            if (!ModelState.IsValid)
            {
                return View(negativeGameEvent);
            }

            await _negativeGameEventSdk.Update(id, negativeGameEvent);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var negativeGameEvent = await _negativeGameEventSdk.Get(id);
            return View(negativeGameEvent);
        }

        [HttpPost("Admin/negativegameevents/delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _negativeGameEventSdk.Delete(id);

            return RedirectToAction("Index");
        }
    }
}

