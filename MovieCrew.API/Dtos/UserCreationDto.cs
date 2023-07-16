using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Dtos;

public record UserCreationDto(long Id, string Name, UserRoles Role = UserRoles.None);
