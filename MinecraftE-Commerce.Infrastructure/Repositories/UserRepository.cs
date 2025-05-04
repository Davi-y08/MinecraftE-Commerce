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
    public class UserRepository : IUserService
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<User> ChangeEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> ChangePassword(string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> ChangePfp(string pfp)
        {
            throw new NotImplementedException();
        }

        public Task<User> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Sale>> getSalesByUser(string idUser)
        {
            var comprasByUser = await _context.Sales
            .Where(s => s.BuyerId == idUser)
            .ToListAsync();

            if (comprasByUser == null)
            {
                return null!;
            }

            return comprasByUser;
        }

        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> RegistrationUser(User userModel)
        {
            throw new NotImplementedException();
        }

        public Task<List<Announcement>> SearchAnnouncement(int id_user)
        {
            throw new NotImplementedException();
        }
    }
}
