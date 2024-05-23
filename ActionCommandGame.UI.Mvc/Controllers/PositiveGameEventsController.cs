using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.UI.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositiveGameEventsController : Controller
    {
        private readonly PositiveGameEventSdk _positiveGameEventSdk;

        public PositiveGameEventsController(PositiveGameEventSdk positiveGameEventSdk)
        {
            _positiveGameEventSdk = positiveGameEventSdk;
        }

        public async Task<IActionResult> Index()
        {
            var positiveGameEvent = await _positiveGameEventSdk.Find();
            return View(positiveGameEvent);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositiveGameEventRequest positiveGameEvent)
        {
            if (!ModelState.IsValid)
            {
                return View(positiveGameEvent);
            }

            await _positiveGameEventSdk.Create(positiveGameEvent);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var positiveGameEvent = await _positiveGameEventSdk.Get(id);

            if (positiveGameEvent is null)
            {
                return RedirectToAction("Index");
            }

            var positiveGameEventRequest = new PositiveGameEventRequest()
            {
                Name = positiveGameEvent.Name,
                Description = positiveGameEvent.Description,
                Zeni = positiveGameEvent.Zeni,
                Experience = positiveGameEvent.Experience,
                Probability = positiveGameEvent.Probability
            };

            return View(positiveGameEventRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, PositiveGameEventRequest positiveGameEvent)
        {
            if (!ModelState.IsValid)
            {
                return View(positiveGameEvent);
            }

            await _positiveGameEventSdk.Update(id, positiveGameEvent);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var positiveGameEvent = await _positiveGameEventSdk.Get(id);
            return View(positiveGameEvent);
        }

        [HttpPost("/positivegameevents/delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _positiveGameEventSdk.Delete(id);

            return RedirectToAction("Index");
        }
    }
}