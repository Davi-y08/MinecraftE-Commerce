using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Services
{
    public class UserService
    {
        private readonly IUserService _userService;

        public UserService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userService.GetUserById(id);    
        }

        public async Task<User> RegistrationUser(User userModel)
        {
            var user = new User();
            user.UserName = userModel.UserName;
            user.Email = userModel.Email;
            user.Pfp = userModel.Pfp;
            user.PasswordHash = userModel.PasswordHash;
            user.Announcements = userModel.Announcements;

            return await _userService.RegistrationUser(user);
        }

        public async Task<User> DeleteUser(int id)
        {
            return await _userService.DeleteUser(id);
        }

        public async Task<User> ChangeEmail(string email)
        {
            var userEmail = email;
            return await _userService.ChangeEmail(userEmail);
        }

        public async Task<User> ChangePassword(string password)
        {
            var userPassword = password;
            return await _userService.ChangePassword(userPassword);
        }
    }
}
