using API.Domain.Services;

namespace API.Application.Services.Contracts;

public interface IServiceRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task AddAsync(Service service, CancellationToken ct);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Service service, CancellationToken ct);

    // Lectura proyectada (sin exponer entidades al Host)
    Task<ServiceProjection?> GetProjectionByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<ServiceProjection>> ListAsync(int skip, int take, CancellationToken ct);
}

public sealed record ServiceProjection(
    Guid Id,
    string Name,
    ushort From,
    ushort To
);
