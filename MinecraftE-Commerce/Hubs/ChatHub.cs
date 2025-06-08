using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace MinecraftE_Commerce.Hub
{
    public class ChatHub : Hub<IHubClient>
    {
        private readonly ISaleService _saleService;
        private readonly IAnnoucementService _annoucementService;
        private readonly UserManager<User> _userService;
        public ChatHub(ISaleService saleService, IAnnoucementService announcementService, UserManager<User> userService)
        {
            _saleService = saleService;
            _annoucementService = announcementService;
            _userService = userService;
        }
        public async Task SendMessage(int chatId, string messageText)
        {
            var senderName = Context.User?.FindFirstValue(JwtRegisteredClaimNames.Name);
            if (string.IsNullOrEmpty(senderName))
                throw new HubException("Usuário não autenticado.");

            var sender = await _userService.FindByNameAsync(senderName);
            if (sender == null)
                throw new HubException("Usuário não encontrado.");

            var saleChat = await _saleService.GetChatByIdAsync(chatId);

            if (saleChat == null || (saleChat.BuyerId != sender.Id && saleChat.ReceiverId != sender.Id))
            {
                throw new HubException("Usuário não pertence ao chat.");
            }

            var message = new Message
            {
                ChatId = chatId,
                MessageString = messageText,
                Send_at = DateTime.UtcNow,
                UserId = sender.Id,
                User = sender
            };

            await _saleService.AddMessageAsync(message);

            foreach (var userId in new[] { saleChat.BuyerId, saleChat.ReceiverId })
            {
                await Clients.User(userId).ReceiveMessage(new
                {
                    chatId = chatId,
                    text = messageText,
                    senderId = sender.Id,
                    sentAt = message.Send_at.ToString("o")
                });
            }
        }


    }
}
