using System.Collections.Immutable;
using Darjeeling.DataContext.Repositories;
using Darjeeling.Helpers;
using Darjeeling.Helpers.LodestoneHelpers;
using Darjeeling.Interfaces;
using Darjeeling.Interfaces.Repositories;
using Darjeeling.Models;
using Darjeeling.Models.Entities;
using Darjeeling.Repositories;
using Darjeeling.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace Darjeeling;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Discord Bot....");
        
        var botToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

        if (string.IsNullOrEmpty(botToken))
        {
            await Console.Error.WriteLineAsync("DISCORD_BOT_TOKEN environment variable is not set.");
            throw new InvalidOperationException("DISCORD_BOT_TOKEN environment variable is not set.");
        }
        
        var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        
        if (string.IsNullOrEmpty(postgresConnectionString))
        {
            await Console.Error.WriteLineAsync("POSTGRES_CONNECTION_STRING environment variable is not set.");
            throw new InvalidOperationException("POSTGRES_CONNECTION_STRING environment variable is not set.");
        }
        
        
        // Build Host
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureHostServices(builder.Services, botToken);
        ConfigureDatabaseService(builder.Services, postgresConnectionString);
        var host = builder.Build();
        InitialiseDatabase(host);

        ConfigureHost(host);
        
        await host.RunAsync();
    }

    private static void ConfigureHostServices(IServiceCollection services, string botToken)
    {
        // Logging
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Trace);
        });

        // Configure AppConfiguration (bot tokens, etc)
        services.Configure<AppConfiguration>(options => { options.botToken = botToken; });

        // Bot Related Services
        services.AddSingleton(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value);
        services.AddSingleton<IPermissionHelpers, PermissionHelpers>();
        services.AddSingleton<ILodestoneApi, LodestoneApi>();
        services.AddSingleton<IMappingHelper, MappingHelper>();
        services.AddScoped<IDomainService, DomainService>();
        services.AddSingleton<IDiscordBackendApiService,DiscordBackendApiService>();
        services.AddSingleton<ICsvHelper, Helpers.CsvHelper>();
        
        
        // Database Services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFCGuildMemberRepository, FCGuildMemberRepository>();
        services.AddScoped<IFCGuildServerRepository, FCGuildServerRepository>();
        services.AddScoped<IFCGuildRoleRepository, FCGuildRoleRepository>();
        services.AddScoped<ILodestoneNameHistoryRepository, LodestoneNameHistoryRepository>();
        services.AddScoped<IDiscordNameHistoryRepository, DiscordNameHistoryRepository>();


        // Intents
        services.AddDiscordGateway(options =>
        {
            options.Intents = GatewayIntents.GuildMessages
                              | GatewayIntents.GuildUsers
                              | GatewayIntents.DirectMessages
                              | GatewayIntents.MessageContent
                              | GatewayIntents.Guilds;
            options.Token = botToken;
        });

        // Slash Command Service
        services.AddApplicationCommands<SlashCommandInteraction, SlashCommandContext, AutocompleteInteractionContext>();

        // Rest Client to support API interaction without responding to an interaction first.
        services.AddSingleton<RestClient>(serviceProvider =>
        {
            var environmentOptions = serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value;
            return new RestClient(environmentOptions.EntityToken);
        });
        
    }
    
    private static void ConfigureHost(IHost host)
    {
        // Additional NetCord Configuration - AddModules to automatically register command modules
        // UseGatewayEventHandlers to automatically register gateway event handlers (Respond to Interactions, etc)
        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayEventHandlers();
        
        // Additional Functionality that will be run right after the bot starts
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(async void () =>
        {
            try
            {
                // Execute Post Startup Utilities
                Console.WriteLine("----------Discord Bot Startup Complete---------");
            }
            catch (Exception e)
            {
                Console.WriteLine(" Error occurred while running post startup utilities: " + e.Message);
                lifetime.StopApplication();
            }
        });
    }
    
    private static void ConfigureDatabaseService(IServiceCollection services, string postgresConnectionString)
    {
        services.AddDbContext<Repositories.DataContext>(options =>
        {
            options.UseNpgsql(postgresConnectionString)
                .LogTo(Console.WriteLine, LogLevel.Information);
        });
    }

    private static void InitialiseDatabase(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            try {
                var dbContext = scope.ServiceProvider.GetRequiredService<Repositories.DataContext>();
                if (!dbContext.Database.CanConnect())
                {
                    throw new ApplicationException("Unable to connect to database.");
                }
                dbContext.Database.Migrate();
                Console.WriteLine("Database initialisation complete.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred while initialising database: " + e.Message);
                throw;
            }
        }
    }
    

}