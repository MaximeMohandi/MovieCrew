namespace MovieCrew.Core.Domain.Movies.Dtos;

public record MovieCreationDto(string Title, string Poster, string Description, long? proposedById);