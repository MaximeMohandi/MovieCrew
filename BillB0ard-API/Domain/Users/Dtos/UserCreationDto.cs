using BillB0ard_API.Domain.Users.Enums;

namespace BillB0ard_API.Domain.Users.Dtos
{
    public record UserCreationDto(string Name, UserRoles Role = UserRoles.None);
}
