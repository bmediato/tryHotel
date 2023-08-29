using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var room = GetRoomById(booking.RoomId);

            var bookingN = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = room
            };

            _context.Bookings.Add(bookingN);
            _context.SaveChanges();

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId);
            var city = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId);

            return new BookingResponse
            {
                BookingId = bookingN.BookingId,
                CheckIn = bookingN.CheckIn,
                CheckOut = bookingN.CheckOut,
                GuestQuant = bookingN.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityId = hotel.CityId,
                        CityName = city.Name,
                        State = city.State
                    }
                }

            };
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = (from book in _context.Bookings
                           where book.BookingId == bookingId && book.User.Email == email
                           select new BookingResponse
                           {
                               BookingId = book.BookingId,
                               CheckIn = book.CheckIn,
                               CheckOut = book.CheckOut,
                               GuestQuant = book.GuestQuant,
                               Room = new RoomDto
                               {
                                   RoomId = book.Room.RoomId,
                                   Name = book.Room.Name,
                                   Capacity = book.Room.Capacity,
                                   Image = book.Room.Image,
                                   Hotel = new HotelDto
                                   {
                                       HotelId = book.Room.Hotel.HotelId,
                                       Name = book.Room.Hotel.Name,
                                       Address = book.Room.Hotel.Address,
                                       CityId = book.Room.Hotel.CityId,
                                       CityName = book.Room.Hotel.City.Name,
                                       State = book.Room.Hotel.City.State
                                   }
                               }
                           }).FirstOrDefault();
            return booking;
        }

        public Room GetRoomById(int RoomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);
            return room;
        }

    }

}