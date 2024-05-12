﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
