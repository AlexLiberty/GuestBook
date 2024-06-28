using GuestBook.Models;

namespace GuestBook.Repository
{
    public interface IUserRepository
    {
        Task RegisterUser(string email, string username, string password);
        Task<User> AuthorizeUser(string email, string password);
        Task<bool> UserExists(string email);
        Task<User> GetUserById(int userId);
    }
}
