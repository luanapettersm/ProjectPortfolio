using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class LoginController(ISystemUserService service,
        ITokenService tokenService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Authentication")]
        public async Task<IActionResult> Authentication([FromBody] AuthenticateModel auth)
        {
            //if (auth == null || string.IsNullOrEmpty(auth.UserName) || string.IsNullOrEmpty(auth.Password))
            //    return BadRequest("usuario e senha sao obrigatorios.");

            //var isValidUser = await service.AuthenticateAsync(auth.UserName, auth.Password);
            //if (!isValidUser)
            //    return Unauthorized("usuario ou senha invalidos.");

            if (auth.UserName == "admin" && auth.Password == "admin")
            {
                var token = await tokenService.GetTokenAsync(auth);
                return Ok(new { token });

            }

            return BadRequest(new { message = "Usuario ou senha invalidos."});
            //return Ok("/Home");
        }
    }
}
