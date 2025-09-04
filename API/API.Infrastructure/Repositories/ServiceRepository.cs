using API.Application.Services.Contracts;
using API.Domain.Services;
using API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories;

public sealed class ServiceRepository(
    Context db
) : IServiceRepository
{
    private readonly Context _db = db;

    public async Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken ct
    ) => await _db.Services.AsNoTracking().AnyAsync(x => x.Name == name, ct);

    public async Task AddAsync(
        Service service,
        CancellationToken ct
    )
    {
        await _db.Services.AddAsync(service, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Service?> GetByIdAsync(
        Guid id,
        CancellationToken ct
    ) => await _db.Services.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task DeleteAsync(
        Service service,
        CancellationToken ct
    )
    {
        _db.Services.Remove(service);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<ServiceProjection?> GetProjectionByIdAsync(
        Guid id,
        CancellationToken ct
    ) => await _db.Services.AsNoTracking()
        .Where(x => x.Id == id)
        .Select(x => new ServiceProjection(x.Id, x.Name, x.PortRange.From, x.PortRange.To))
        .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<ServiceProjection>> ListAsync(
        int skip,
        int take,
        CancellationToken ct
    ) => await _db.Services.AsNoTracking()
        .OrderBy(x => x.Name)
        .Skip(skip).Take(take)
        .Select(x => new ServiceProjection(x.Id, x.Name, x.PortRange.From, x.PortRange.To))
        .ToListAsync(ct);
}
