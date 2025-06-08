using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infrastructure.Data;

namespace MinecraftE_Commerce.Infrastructure.Repositories
{
    public class ChatRepo : ChatService

    {
    private readonly AppDbContext _context;

    public ChatRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> CreateChat(Chat chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat?> GetChatByParticipantsAsync(string buyerId, string receiverId, int announcementId)
        {
            return await _context.Chats
                .FirstOrDefaultAsync(c =>
                    c.BuyerId == buyerId &&
                    c.ReceiverId == receiverId &&
                    c.AnnouncementId == announcementId);
        }
    }
}
