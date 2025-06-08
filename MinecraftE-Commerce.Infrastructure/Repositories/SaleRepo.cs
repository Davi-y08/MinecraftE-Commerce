using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infrastructure.Data;

namespace MinecraftE_Commerce.Infrastructure.Repositories
{
    public class SaleRepo : ISaleService
    {
        private readonly AppDbContext _context;

        public SaleRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateSale(Sale saleModel)
        {
            await _context.AddAsync(saleModel);
            await _context.SaveChangesAsync();

            return saleModel;
        }

        public async Task<List<Sale>> GetAllSales()
        {
            var sales = await _context.Sales.ToListAsync();

            return sales;
        }

        public async Task<Chat?> GetChatByIdAsync(int chatId)
        {
            return await _context.Chats
            .Include(c => c.Buyer)
            .Include(c => c.Receiver)
            .Include(c => c.Sale)
            .FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<List<Sale>> IsBought(int idAnnouncement)
        {
            var sales = await _context.Sales.Where(s => s.AnnouncementId == idAnnouncement).ToListAsync();

            if (sales == null)
            {
                return null!;
            }

            return sales;
        }

        public Task<Sale> StatusSale(bool status)
        {
            throw new NotImplementedException();
        }
        public async Task AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

    }
}
