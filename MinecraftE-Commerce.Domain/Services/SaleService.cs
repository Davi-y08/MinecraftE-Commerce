using System.Diagnostics.Contracts;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Services
{
    public class SaleService
    {
        private readonly ISaleService? _saleService;
        private readonly IAnnoucementService? _annoucementService;

        public async Task<List<Sale>> GetAllSales()
        {
            return await _saleService!.GetAllSales();
        }

        public async Task<Sale> CreateSale(Sale saleModel, int announcementId, int buyerId, int receiverId, Announcement priceAnn)
        {
            var announcement = _annoucementService!.GetAnnouncementById(announcementId);
    
            var newSale = new Sale();
            newSale.Id = saleModel.Id;
            newSale.SaledOn = saleModel.SaledOn;
            newSale.AnnouncementPrice = priceAnn.PriceService;
            newSale.AnnouncementId = announcementId;
            newSale.BuyerId = buyerId;
            newSale.ReceiverId = receiverId;

            return await _saleService!.CreateSale(newSale);
        }
    }
}
