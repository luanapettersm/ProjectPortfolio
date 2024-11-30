using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class LoginController(ISystemUserService service) : Controller
    {
        //public IActionResult Index()
        //{
        //    return View("/Index");
        //}

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateModel auth)
        {
            if (auth == null || string.IsNullOrEmpty(auth.UserName) || string.IsNullOrEmpty(auth.Password))
                return BadRequest("usuario e senha sao obrigatorios.");

            var isValidUser = await service.AuthenticateAsync(auth.UserName, auth.Password);
            if (!isValidUser)
                return Unauthorized("usuario ou senha invalidos.");

            return Ok("/Home");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            return Redirect("/Home");
        }
    }
}
