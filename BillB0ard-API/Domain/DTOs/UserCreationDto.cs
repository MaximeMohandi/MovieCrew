using BillB0ard_API.Domain.Enums;

namespace BillB0ard_API.Domain.DTOs
{
    public record UserCreationDto(string Name, UserRoles Role = 0);
}
