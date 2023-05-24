using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using Moq;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.API.Test.Integration.Movie;

public class MovieEndpointTestBase
{
    protected readonly JsonSerializerOptions _jsonOptions;
    protected HttpClient _client;
    protected Mock<IMovieService> _movieService;

    internal MovieEndpointTestBase()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    [OneTimeSetUp]
    public void Init()
    {
        _client = new IntegrationTestServer<IMovieService>(_movieService).CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            MockJwtTokens.GenerateJwtToken());
    }

    [SetUp]
    public void SetUp()
    {
        _movieService = new Mock<IMovieService>();
    }
}
