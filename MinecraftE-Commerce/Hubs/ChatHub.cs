using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace MinecraftE_Commerce.Hub
{
    public class ChatHub : Hub<HubProvider>
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
            var senderName = Context.User!.FindFirstValue(JwtRegisteredClaimNames.Name);
            var sender = await _userService.FindByNameAsync(senderName!);
            var saleChat = await _saleService.GetChatByIdAsync(chatId);

            if (saleChat == null || (saleChat.BuyerId != sender!.Id && saleChat.ReceiverId != sender.Id))
            {
                throw new HubException("Usuário não pertence ao chat.");
            }

            var message = new Message
            {
                ChatId = chatId,
                MessageString = messageText,
                Send_at = DateTime.Now,
                UserId = sender.Id,
                User = sender
            };

            await Clients.Users(saleChat.BuyerId, saleChat.ReceiverId)
            .ReceiveMessage(new
            {
            ChatId = chatId,
            Text = messageText,
            SenderId = sender.Id,
            SentAt = message.Send_at
            });
        }
    }
}
