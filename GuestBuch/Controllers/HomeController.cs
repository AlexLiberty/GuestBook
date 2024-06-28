using Microsoft.AspNetCore.Mvc;
using GuestBook.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuestBookContext _context;

        public HomeController(GuestBookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages.Include(m => m.User).ToList();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(string messageText)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var message = new Message
                {
                    UserId = int.Parse(userId),
                    MessageText = messageText
                };
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

