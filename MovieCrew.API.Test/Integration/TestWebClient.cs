using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;

namespace MovieCrew.API.Test.Integration;

public class IntegrationTestServer<T> : WebApplicationFactory<Program> where T : class
{
    private readonly Mock<T> _mockedService;

    public IntegrationTestServer(Mock<T> mockedService)
    {
        _mockedService = mockedService;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            IdentityModelEventSource.ShowPII = true;
            services.RemoveAll<T>();
            services.AddScoped<T>(sp => _mockedService.Object);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddScheme<JwtBearerOptions, >(JwtBearerDefaults.AuthenticationScheme, options => { });

        });
    }
}
