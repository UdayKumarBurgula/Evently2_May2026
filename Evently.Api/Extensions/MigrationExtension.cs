using Events.Module.Events.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading;

namespace Evently.Api.Extensions;

internal static class MigrationExtension
{
    internal static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        ILogger logger = loggerFactory.CreateLogger("Migration");
        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        string? connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("No connection string named 'Database' was found. Skipping migrations.");
            return;
        }

        const int maxAttempts = 5;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                ApplyMigration<EventsDbContext>(scope, logger);
                logger.LogInformation("Database migrations applied successfully.");
                break;
            }
            catch (Exception ex)
            {
                // If this is an authentication error, surface a clear message and stop retries.
                if (ex is PostgresException pgEx && pgEx.SqlState == "28P01")
                {
                    logger.LogError(ex, "Postgres authentication failed (SqlState=28P01). Check username/password in your connection string and the container environment variables.");
                    throw;
                }

                // For other errors, retry with backoff while logging context.
                logger.LogWarning(ex, "Attempt {Attempt}/{MaxAttempts} to apply migrations failed.", attempt, maxAttempts);

                if (attempt == maxAttempts)
                {
                    logger.LogError(ex, "All attempts to apply migrations have failed. Verify the database is accessible and the connection string is correct.");
                    throw;
                }

                // Exponential backoff with cap
                int delaySeconds = (int)Math.Min(30, Math.Pow(2, attempt));
                logger.LogInformation("Waiting {DelaySeconds}s before next attempt to apply migrations...", delaySeconds);
                Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
            }
        }
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope, ILogger logger) where TDbContext : DbContext
    {
        using TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        logger.LogInformation("Attempting to connect to database at {DataSource} and run migrations.", dbContext.Database.GetDbConnection().DataSource);
        dbContext.Database.Migrate();
    }
}
