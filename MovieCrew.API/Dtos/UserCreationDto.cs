using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Dtos;

public record UserCreationDto(string Name, UserRoles Role = UserRoles.None);
