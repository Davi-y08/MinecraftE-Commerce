﻿using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Interfaces
{
    public interface ISaleService
    {
        Task<List<Sale>> GetAllSales();
        Task<Sale> CreateSale(Sale saleModel, int idAnnouncement, int idBuyer);
        Task<Sale> StatusSale(bool status);
    }
}
