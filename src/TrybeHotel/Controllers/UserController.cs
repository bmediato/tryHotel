using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var userType = token?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Role)?.Value;

            var users = _repository.GetUsers();

            if (users == null)
            {
                return Unauthorized();
            }

            return Ok(users);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            var email = _repository.GetUserByEmail(user.Email);

            if (email != null)
            {
                return Conflict(new { message = "User email already exists" });
            }
            var newUser = _repository.Add(user);
            return Created("", newUser);
        }
    }
}