using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class LoginController(ISystemUserRepository repository,
        ITokenService tokenService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Authentication")]
        public async Task<IActionResult> Authentication(AuthenticateModel auth)
        {
            if (auth == null || string.IsNullOrEmpty(auth.UserName) || string.IsNullOrEmpty(auth.Password))
                return BadRequest("Usuário e senha são obrigatórios.");

            var user = await repository.GetUserByUserName(auth.UserName);

            if (user == null)
                return BadRequest("Usuário não localizado no sistema.");

            var password = BCrypt.Net.BCrypt.Verify(auth.Password, user.Password);

            if (auth.UserName != user.UserName || !password)
                return BadRequest(new { message = "Usuario ou senha inválidos." });

            var token = await tokenService.GetTokenAsync(auth);

            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return RedirectToAction("Index", "Home");
        }
    }
}
