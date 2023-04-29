using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieCrew.Core.Data;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.API.Test.Integration.Movie;

public class MovieGetAllTest
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public MovieGetAllTest()
    {
        var webAppFactory = new CustomWebApplicationFactory();
        _client = webAppFactory.CreateDefaultClient();
    }

    [Test]
    public async Task FetchAllMovie()
    {
        var expected = new List<MovieEntity>();

        var response = await _client.GetAsync("/api/movie/random");
        var content = await response.Content.ReadFromJsonAsync<List<MovieEntity>>();

        Assert.That(content, Is.EqualTo(expected));
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services => { services.AddTransient<AppDbContext>(); });

        builder.ConfigureServices(services =>
        {
            services.AddTransient<DbContextOptions>();
            services.AddTransient<AppDbContext>();
            services.AddTransient<IMovieRepository, MovieRepository>();
            var test = new Mock<IThirdPartyMovieDataProvider>();
            services.AddTransient<IThirdPartyMovieDataProvider>(provider => { return test.Object; });
            services.AddTransient<MovieService>();
        });
    }
}
