using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor(
    ILogger<AuditSaveChangesInterceptor> logger
) : SaveChangesInterceptor
{
    private readonly ILogger<AuditSaveChangesInterceptor> _logger = logger;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        => base.SavingChanges(eventData, result);

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        => base.SavingChangesAsync(eventData, result, cancellationToken);

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        WriteAuditLogs(eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        WriteAuditLogs(eventData.Context);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void WriteAuditLogs(DbContext? ctx)
    {
        if (ctx is null) return;

        var entries = ctx.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Deleted)
            .ToList();

        foreach (var e in entries)
        {
            var entityName = e.Entity.GetType().Name;

            if (e.State == EntityState.Added)
            {
                // Ejemplo: log estructurado
                _logger.LogInformation("AUDIT Insert {Entity} Key={Key} Values={Values}",
                    entityName,
                    TryGetKey(e),
                    e.CurrentValues.Properties.ToDictionary(p => p.Name, p => e.CurrentValues[p]));
            }
            else if (e.State == EntityState.Deleted)
            {
                _logger.LogInformation("AUDIT Delete {Entity} Key={Key} OriginalValues={Values}",
                    entityName,
                    TryGetKey(e),
                    e.OriginalValues.Properties.ToDictionary(p => p.Name, p => e.OriginalValues[p]));
            }
        }
    }

    private static object? TryGetKey(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry e)
    {
        var key = e.Metadata.FindPrimaryKey();
        if (key is null) return null;
        if (key.Properties.Count == 1)
            return e.Property(key.Properties[0].Name).CurrentValue ?? e.Property(key.Properties[0].Name).OriginalValue;
        return key.Properties.Select(p => e.Property(p.Name).CurrentValue).ToArray();
    }
}
