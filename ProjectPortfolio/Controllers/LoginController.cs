using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class LoginController(ITokenService tokenService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return Redirect("/Home");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate(AuthenticateModel login)
        {
            if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Usuário e senha são obrigatórios.");

            var isValidUser = ValidateUser(login.UserName, login.Password);
            if (!isValidUser)
                return Unauthorized("Usuário ou senha inválidos.");

            var token = await tokenService.GetTokenAsync(login);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Falha na geração do token.");

            return Ok(new { Token = token });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            //
            //

            return Redirect("/Home");
        }

    }
}
