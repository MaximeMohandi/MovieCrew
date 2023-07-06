using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Services;

public interface IThirdPartyMovieDataProvider
{
    Task<MovieMetadataEntity> GetDetails(string title);
}
