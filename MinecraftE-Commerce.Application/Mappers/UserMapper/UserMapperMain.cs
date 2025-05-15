using MinecraftE_Commerce.Application.Dtos.UserDto;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Application.Mappers.UserMapper
{
    public static class UserMapperMain
    {
        public static UserDisplay MapToUserDisplay(this User userModel)
        {
            return new UserDisplay
            {
                Pfp = userModel.Pfp,
                UserName = userModel.Pfp
            };
        }

        public static User MapToChangeEmail(this changeEmail emailDto)
        {
            return new User
            {
                Email = emailDto.newEmail,
            };
        }
    }
}
