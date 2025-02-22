using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Domain.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(User userModel);
    }
}
