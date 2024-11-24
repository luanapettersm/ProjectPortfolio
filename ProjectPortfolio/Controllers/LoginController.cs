using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    public class LoginController(ITokenService tokenService) : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Necessário informar usuário e senha para realizar o login.");

            var token = await tokenService.GetTokenAsync(model);

            if(token == "" || token == null)
                return Unauthorized();
            //var user = await systemUserRepository.GetByUserName(model.UserName);

            //if (user == null)
            //    return NotFound(new { message = "Usuário ou senha inválidos" });


            //user.Password = "";

            return Ok(token);
        }
    }
}
