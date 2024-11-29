using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class LoginController(ISystemUserService service) : Controller
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthenticateModel auth)
        {
            if (string.IsNullOrEmpty(auth.UserName) || string.IsNullOrEmpty(auth.Password))
                return BadRequest("Usuário e senha são obrigatórios.");
            
            var isValidUser = await service.AuthenticateAsync(auth.UserName, auth.Password);
            if (!isValidUser)
                return Unauthorized("Usuário ou senha inválidos.");

            return Redirect("/Home");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            return Redirect("/Home");
        }

    }
}
