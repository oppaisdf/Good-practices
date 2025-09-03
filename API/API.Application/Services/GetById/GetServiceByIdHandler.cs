using API.Application.Services.Contracts;
using API.Application.Services.DTOs;
using API.Domain.Common;

namespace API.Application.Services.GetById;

public sealed class GetServiceByIdHandler(
    IServiceRepository repo
)
{
    private readonly IServiceRepository _repo = repo;

    public async Task<Result<ServiceDTO>> HandleAsync(
        GetServiceByIdQuery query,
        CancellationToken ct
    )
    {
        var proj = await _repo.GetProjectionByIdAsync(query.Id, ct);
        if (proj is null) return Result<ServiceDTO>.Failure("Service not found");
        return Result<ServiceDTO>.Success(new ServiceDTO(proj.Id, proj.Name, proj.From, proj.To));
    }
}
