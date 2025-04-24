using IntegrationAPISample.Application.Abstractions;
using IntegrationAPISample.Application.Services;
using IntegrationAPISample.Application.Settings;
using IntegrationAPISample.Infrastructure.BootsrapExtensions;
using IntegrationAPISample.Infrastructure.ExternalApis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace IntegrationAPISample.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Description = "Enter token as: Bearer {your token}",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference 
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" 
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        RegisterClientApiSettings(builder);

        RegisterServices(builder);

        RegisterResilliencyPolicy(builder);

        RegisterAuth(builder);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(error.Error, "Unhandled Exception");

                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "An unexpected error occurred. Please try again later."
                    });
                }
            });
        });


        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void RegisterAuth(WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        builder.Services.Configure<JwtSettings>(jwtSettings);

        byte[] key = GetJwtKeyFromSettings(jwtSettings);

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader) && !authHeader.StartsWith("Bearer "))
                        {
                            context.Token = authHeader;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();
    }

    private static void RegisterClientApiSettings(WebApplicationBuilder builder)
    {
        var openWeatherSetting = builder.Configuration.GetSection("ExternalApis:OpenWeather");
        var hotelSetting = builder.Configuration.GetSection("ExternalApis:HotelAPI");
        var flightSetting = builder.Configuration.GetSection("ExternalApis:AviationStack");

        builder.Services.Configure<WeatherSettings>(openWeatherSetting);
        builder.Services.Configure<HotelSettings>(hotelSetting);
        builder.Services.Configure<FlightSettings>(flightSetting);

        builder.Services.AddHttpClient<IWeatherClient, WeatherClient>(client =>
        {
            var options = openWeatherSetting.Get<WeatherSettings>()!;
            client.BaseAddress = new Uri(options.Endpoint!);
        });

        builder.Services.AddHttpClient<IHotelClient, HotelClient>(client =>
        {
            var options = hotelSetting.Get<HotelSettings>()!;
            client.BaseAddress = new Uri(options.Endpoint);
        });

        builder.Services.AddHttpClient<IFlightClient, FlightClient>(client =>
        {
            var options = flightSetting.Get<FlightSettings>()!;
            client.BaseAddress = new Uri(options.Endpoint);
        });
    }

    private static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IWeatherClient, WeatherClient>();
        builder.Services.AddScoped<IHotelClient, HotelClient>();
        builder.Services.AddScoped<IFlightClient, FlightClient>();
        builder.Services.AddScoped<IAggregationService, AggregationService>();
    }

    private static void RegisterResilliencyPolicy(WebApplicationBuilder builder)
    {
        builder.Services
            .AddResilientPolicy<IWeatherClient, WeatherClient>()
            .AddResilientPolicy<IFlightClient, FlightClient>()
            .AddResilientPolicy<IHotelClient, HotelClient>();
    }

    private static byte[] GetJwtKeyFromSettings(IConfigurationSection jwtSection)
    {
        if (jwtSection == null)
        {
            throw new InvalidOperationException("JWT configuration is missing. Please check appsettings.json.");
        }

        var jwtSettings = jwtSection.Get<JwtSettings>();

        if (jwtSettings == null ||
            string.IsNullOrWhiteSpace(jwtSettings.Key) ||
            jwtSettings.Key.Length < 32)
        {
            throw new InvalidOperationException("JWT configuration is invalid. Please check appsettings.json.");
        }

        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

        return key;
    }
}
