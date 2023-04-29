namespace MovieCrew.Core.Domain.Movies.Exception;

public class ThirdPartyMovieProviderException : System.Exception
{
    public ThirdPartyMovieProviderException(string message) : base(message)
    {
    }
}

public class NoMetaDataFoundException : ThirdPartyMovieProviderException
{
    public NoMetaDataFoundException(string title) : base($"No metadata found for the movie {title}.")
    {
    }
}