using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]

    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var emailUser = token?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;

            var room = _repository.GetRoomById(bookingInsert.RoomId);
            if (room == null || bookingInsert.GuestQuant > room.Capacity)
            {
                return BadRequest(new { message = "Guest quantity over rooom capacity" });
            }

            var reserve = _repository.Add(bookingInsert, emailUser);
            return Created("", reserve);
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid)
        {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var emailUser = token?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;

            var booking = _repository.GetBooking(Bookingid, emailUser);

            if (booking == null)
            {
                return Unauthorized();
            }

            return Ok(booking);
        }
    }
}