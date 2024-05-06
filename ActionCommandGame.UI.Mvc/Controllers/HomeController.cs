using ActionCommandGame.UI.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ActionCommandGame.UI.Mvc.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
