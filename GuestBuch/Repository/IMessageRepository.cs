using System.Collections.Generic;
using System.Threading.Tasks;
using GuestBook.Models;

namespace GuestBook.Repository
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetMessagesWithUsers();
        Task AddMessage(Message message);
    }
}
