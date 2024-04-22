using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;


namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignIn(UserSignInRequest request)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            return Ok();
        }
    }
}
