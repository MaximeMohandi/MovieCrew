using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.Core.Domain.Users.Dtos;

public record UserCreationDto(string Name, UserRoles Role = UserRoles.None);