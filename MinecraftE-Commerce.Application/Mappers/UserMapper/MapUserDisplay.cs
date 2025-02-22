using MinecraftE_Commerce.Application.Dtos.UserDto;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Mappers.UserMapper
{
    public static class MapUserDisplay
    {
        public static UserDisplay MapToUserDisplay(User userModel)
        {
            return new UserDisplay
            {
                Pfp = userModel.Pfp,
                UserName = userModel.Pfp
            };
        }
    }
}
