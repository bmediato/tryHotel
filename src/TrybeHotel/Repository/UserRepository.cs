using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            if (user != null)
            {
                return new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    UserType = user.UserType
                };
            }
            return null;
        }
        public UserDto Add(UserDtoInsert user)
        {
            var userN = new User
            {
                Name = user.Name,
                Email = user.Email,
                UserType = "client"
            };
            _context.Users.Add(userN);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = userN.UserId,
                Name = userN.Name,
                Email = userN.Email,
                UserType = userN.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user != null)
            {
                return new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    UserType = user.UserType
                };
            }
            return null;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return _context.Users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            }).ToList();
        }

    }
}