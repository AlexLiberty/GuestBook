using GuestBook.Models;
using GuestBook.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public HomeController(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _messageRepository.GetMessagesWithUsers();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(string messageText)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var user = await _userRepository.GetUserById(int.Parse(userId));
                if (user != null)
                {
                    var message = new Message
                    {
                        UserId = user.Id,
                        MessageText = messageText
                    };
                    await _messageRepository.AddMessage(message);
                }
            }
            return RedirectToAction("Index");
        }
    }
}

