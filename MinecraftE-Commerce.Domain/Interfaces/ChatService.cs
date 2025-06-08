using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Interfaces
{
    public interface ChatService
    {
        Task<Chat?> CreateChat(Chat chat);
        Task<Chat?> GetChatByParticipantsAsync(string buyerId, string receiverId, int announcementId);
    }
}
