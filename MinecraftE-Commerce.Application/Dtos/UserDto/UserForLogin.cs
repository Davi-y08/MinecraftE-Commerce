using System.ComponentModel.DataAnnotations;

namespace MinecraftE_Commerce.Application.Dtos.UserDto
{
    public class UserForLogin
    {
        [Required(ErrorMessage = "Email is required for login")]
        public string EmailForLogin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required for login")]
        public string PasswordForLogin { get; set; } = string.Empty;

        public string ConfirmedPassword { get; set; } = string.Empty;
    }
}
