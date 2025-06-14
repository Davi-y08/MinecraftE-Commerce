﻿using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Interfaces
{
    public interface ISaleService
    {
        Task<List<Sale>> GetAllSales();
        Task<Sale> CreateSale(Sale saleModel);
        Task<Sale> StatusSale(bool status);
        Task<List<Sale>> IsBought(int idAnnouncement);
        Task<Chat?> GetChatByIdAsync(int chatId);

        Task AddMessageAsync(Message message);
    }
}
