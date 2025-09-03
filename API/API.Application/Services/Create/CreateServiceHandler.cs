using API.Application.Services.Contracts;
using API.Application.Services.DTOs;
using API.Domain.Common;
using API.Domain.Services;

namespace API.Application.Services.Create;

public sealed class CreateServiceHandler(
    IServiceRepository repo
)
{
    private readonly IServiceRepository _repo = repo;

    public async Task<Result<ServiceDTO>> HandleAsync(
        CreateServiceCommand request,
        CancellationToken ct
    )
    {
        // Validación de caso de uso (antes de tocar repos)
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<ServiceDTO>.Failure("Name is required");
        if (request.From > request.To)
            return Result<ServiceDTO>.Failure("'From' must be <= 'To'");

        if (await _repo.ExistsByNameAsync(request.Name.Trim(), ct))
            return Result<ServiceDTO>.Failure("A Service with the same name already exists");

        var pr = PortRange.Create(request.From, request.To);
        var created = Service.Create(request.Name.Trim(), pr);
        if (!created.IsSuccess) return Result<ServiceDTO>.Failure(created.Error!);

        await _repo.AddAsync(created.Value!, ct); // una operación = un SaveChanges en repo/infra

        var dto = new ServiceDTO(created.Value!.Id, created.Value!.Name, pr.From, pr.To);
        return Result<ServiceDTO>.Success(dto);
    }
}
