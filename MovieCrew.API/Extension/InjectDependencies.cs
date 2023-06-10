using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Repository;
using MovieCrew.Core.Domain.Authentication.Services;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Ratings.Repository;
using MovieCrew.Core.Domain.Ratings.Services;
using MovieCrew.Core.Domain.Users.Repository;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Extension;

public static class InjectDependencies
{
    public static void InjectDomainServices(this IServiceCollection services)
    {
        InjectDatabase(services);
        InjectRepository(services);
        InjectHelpers(services);
        InjectService(services);
    }

    private static void InjectDatabase(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("MovieDb"); });
    }

    private static void InjectRepository(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRateRepository, RateRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
    }

    private static void InjectHelpers(IServiceCollection services)
    {
        services.AddScoped<JwtConfiguration>();
    }

    private static void InjectService(IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISpectatorService, SpectatorService>();
        services.AddScoped<IRatingService, RatingService>();
    }
}
