using API.Domain.Services;
using API.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Persistence;

public sealed class Context(
    DbContextOptions<Context> options,
    AuditSaveChangesInterceptor audit
) : DbContext(options)
{
    private readonly AuditSaveChangesInterceptor _audit = audit;

    public DbSet<Service> Services => Set<Service>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder
    ) => optionsBuilder.AddInterceptors(_audit);

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    ) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
}
