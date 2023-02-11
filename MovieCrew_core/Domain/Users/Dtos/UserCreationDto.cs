using MovieCrew_core.Domain.Users.Enums;

namespace MovieCrew_core.Domain.Users.Dtos
{
    public record UserCreationDto(string Name, UserRoles Role = UserRoles.None);
}
