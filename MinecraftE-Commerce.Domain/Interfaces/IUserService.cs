using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> RegistrationUser(User userModel);
        Task<User> ChangeEmail(string email);
        Task<User> ChangePassword(string password);
        Task<User> ChangePfp(string pfp);
        Task<User> DeleteUser(int id);
        Task<List<Announcement>> SearchAnnouncement(int id_user);
    }
}
