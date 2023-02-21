using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Services
{
    //implements The Movie Data Base as external movie data provider
    public class TmdbMovieData : IThirdPartyMovieData
    {
        public TmdbMovieData()
        {

        }

        public MovieMetadataEntity SearchMovieData(string movieTitle)
        {
            return new("https://link", "lorem ipsum ..", 0M, 34000000.00M);
        }
    }
}
