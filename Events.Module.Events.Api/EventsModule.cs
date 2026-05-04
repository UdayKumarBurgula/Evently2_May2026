using Events.Module.Events.Api.Database;
using Events.Module.Events.Api.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Module.Events.Api;

public static class EventsModule
{
    public static void MapEndPoint(IEndpointRouteBuilder app) 
    { 
        CreateEvent.MapEndPoints(app);
        GetEvent.MapEndPoint(app);
    }

    public static IServiceCollection AddEventsModule(
        this IServiceCollection services, 
        IConfiguration  configuration) 
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<EventsDbContext>(options =>
            options.UseNpgsql(
                    databaseConnectionString,
                    npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events)

                ).UseSnakeCaseNamingConvention()

        );
        return services;
    }
}
