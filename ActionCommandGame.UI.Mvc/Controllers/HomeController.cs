using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.UI.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View("Error");
        } 
    }
}
