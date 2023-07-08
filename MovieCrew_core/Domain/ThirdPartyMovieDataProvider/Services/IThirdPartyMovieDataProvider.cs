using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Services;

public interface IThirdPartyMovieDataProvider
{
    Task<MovieMetadataEntity> GetDetails(string title);
}
