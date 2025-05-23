﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovieCrew.Core.Domain.Authentication.Model;
using Serilog;

namespace MovieCrew.API.Extension;

public static class ConfigureAuthenticationExtension
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration(
            configuration.GetValue<string>("JwtConfiguration:Passphrase"),
            configuration.GetValue<string>("JwtConfiguration:Issuer"),
            configuration.GetValue<string>("JwtConfiguration:Audience"),
            configuration.GetValue<int>("JwtConfiguration:MaxTokenValidationDays"));

        services.AddSingleton(jwtConfiguration);

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Passphrase))
                };
            });

        services.AddLogging(builder =>
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            builder.AddSerilog(logger, true);
        });
    }
}
