namespace MovieCrew.Core.Domain.ThirdPartyMovieProvider.Exception
{
    public class ThirdPartyMovieProviderException : System.Exception
    {
        public ThirdPartyMovieProviderException(string message) : base(message) { }
    }

    public class NoMetaDataFound : ThirdPartyMovieProviderException
    {
        public NoMetaDataFound(string title) : base($"No metadata found for the movie {title}.") { }
    }
}
