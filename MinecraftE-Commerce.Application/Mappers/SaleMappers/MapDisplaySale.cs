using MinecraftE_Commerce.Application.Dtos.SaleDto;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Mappers.SaleMappers
{
    public static class MapDisplaySale
    {
        public static DisplaySaleDto MapToSaleDisplay(Sale saleModel)
        {
            return new DisplaySaleDto
            {
                idSale = saleModel.Id,
                saledOn = saleModel.SaledOn,
                announcementId = saleModel.AnnouncementId,
                buyerId = saleModel.BuyerId,
                receiverId = saleModel.ReceiverId,
            };
        }
    }
}
