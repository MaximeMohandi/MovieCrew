namespace MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Exception;

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

public class CantFetchThirdPartyApiException : ThirdPartyMovieProviderException
{
    public CantFetchThirdPartyApiException() : base("An error occurred while fetching the third party API.")
    {
    }
}
