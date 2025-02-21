using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace MinecraftE_Commerce.Application.Dtos.UserDto
{
    public class CreateUser
    {
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(5, ErrorMessage = "Minimum 5 characters required")]
        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Minimum 8 character required")]
        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed")]
        public string Password { get; set; }= string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Differents password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public required IFormFile Pfp { get; set; } 
    }
}

