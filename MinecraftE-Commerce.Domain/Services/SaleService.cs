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

        public async Task<Sale> CreateSale(Sale saleModel, int idAnnouncement, string receiverId, string buyerId)
        {
            var announcementSearch = await _annoucementService!.GetAnnouncementById(idAnnouncement);
                
            if (announcementSearch == null)
            {
                throw new Exception("Anúncio não encontrado!");
            }

            receiverId = announcementSearch.UserId;

            var newSale = new Sale
            {
                SaledOn = DateTime.Now,
                Announcement = announcementSearch,
                AnnouncementPrice = announcementSearch.PriceService,
                AnnouncementId = announcementSearch.Id,
                ReceiverId = receiverId,
                BuyerId = buyerId,
            };

            return await _saleService!.CreateSale(newSale);
        }
    }
}
