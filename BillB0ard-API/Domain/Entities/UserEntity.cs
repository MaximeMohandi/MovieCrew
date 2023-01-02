using BillB0ard_API.Domain.Enums;

namespace BillB0ard_API.Domain.Entities
{
    public record UserEntity(long Id, string Name, UserRoles Role = UserRoles.None);
}