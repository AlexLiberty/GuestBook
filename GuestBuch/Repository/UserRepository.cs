using GuestBook.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Repository
{
    public class UserRepository : IRepository
    {
        private readonly GuestBookContext _context;

        public UserRepository(GuestBookContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(string email, string username, string password)
        {
            string salt = SecurityHelper.GenerateSalt(16);
            string hashedPassword = SecurityHelper.HashPassword(password, salt, 10000, 32);

            var user = new User
            {
                Email = email,
                Name = username,
                Password = hashedPassword,
                Salt = salt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AuthorizeUser(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                string hashedPassword = SecurityHelper.HashPassword(password, user.Salt, 10000, 32);
                if (user.Password == hashedPassword)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
