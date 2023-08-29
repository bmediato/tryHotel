using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;
        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var userLogin = _repository.Login(login);
            if (userLogin == null)
            {
                return Unauthorized(new { message = "Incorrect e-mail or password" });
            }
            var tokenGen = new TokenGenerator().Generate(userLogin);
            return Ok(new { token = tokenGen });
        }
    }
}