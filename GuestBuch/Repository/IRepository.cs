using GuestBook.Models;

namespace GuestBook.Repository
{
    public interface IRepository
    {
        Task RegisterUser(string email, string username, string password);
        Task<User> AuthorizeUser(string email, string password);
    }
}
