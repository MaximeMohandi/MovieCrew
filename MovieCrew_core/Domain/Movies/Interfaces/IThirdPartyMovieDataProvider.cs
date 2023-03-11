using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Interfaces
{
    public interface IThirdPartyMovieDataProvider
    {
        Task<MovieMetadataEntity> GetDetails(string title);
    }
}