using ChatApp.Models.Message;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;

        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }

        public static List<KeyValuePair<string, string>> sender_messages = 
            new List<KeyValuePair<string, string>>();

        public IActionResult Show()
        {
            if(sender_messages.Count() < 1)
            {
                return View(new ChatViewModel());
            }

            var chatModel = new ChatViewModel()
            {
                Messages = sender_messages
                .Select(m => new MessageViewModel()
                {
                    Sender = m.Key,
                    MessageText = m.Value
                })
                .ToList()
            };

            return View(chatModel);
        }

        [HttpPost]
        public IActionResult Send(ChatViewModel chat)
        {
            var newMessage = chat.CurrentMessage;

            sender_messages.Add(new KeyValuePair<string, string>
                (newMessage.Sender, newMessage.MessageText));

            _logger.LogInformation($"Message was sent from {newMessage.Sender}");

            return RedirectToAction("Show");
        }
    }
}
