using Solid.Aim.Contracts;
using Solid.Aim.Models;

namespace Solid.Aim.Mappings
{
    internal static class UserMappings
    {
        public static User ToUser(this UserDto dto, string id = null)
        {
            return dto == null
                ? null
                : new User
                {
                    Age = dto.Age ?? 0,
                    Name = dto.Name,
                    Id = id
                };
        }

        public static UserDto ToUserDto(this User user)
        {
            return user == null
                ? null
                : new UserDto
                {
                    Age = user.Age ?? 0,
                    Name = user.Name,
                };
        }
    }
}
