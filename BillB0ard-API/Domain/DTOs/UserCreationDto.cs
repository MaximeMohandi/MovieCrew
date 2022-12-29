using BillB0ard_API.Domain.Enums;

namespace BillB0ard_API.Domain.DTOs
{
    public record UserCreationDto(long Id, string Name, UserRoles Role = UserRoles.None);
}
