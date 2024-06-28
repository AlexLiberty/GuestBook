using GuestBook.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Repository
{
    public class MessageRepository: IMessageRepository
    {
        private readonly GuestBookContext _context;

        public MessageRepository(GuestBookContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetMessagesWithUsers()
        {
            return await _context.Messages.Include(m => m.User).ToListAsync();
        }

        public async Task AddMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
