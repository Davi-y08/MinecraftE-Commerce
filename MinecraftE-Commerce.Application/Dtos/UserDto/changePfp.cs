using Microsoft.AspNetCore.Http;

namespace MinecraftE_Commerce.Application.Dtos.UserDto
{
    public class changePfp
    {
        public required IFormFile newPfp { get; set; }
    }
}
