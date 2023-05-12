﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        builder.UseEnvironment("IntegrationTest");
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<T>();
            services.AddScoped<T>(sp => _mockedService.Object);
        });
    }
}