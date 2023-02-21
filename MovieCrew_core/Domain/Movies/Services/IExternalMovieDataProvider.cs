using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Services
{
    public interface IThirdPartyMovieData
    {
        MovieMetadataEntity SearchMovieData(string movieTitle);
    }
}
