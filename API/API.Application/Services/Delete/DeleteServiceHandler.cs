using API.Application.Services.Contracts;
using API.Domain.Common;

namespace API.Application.Services.Delete;

public sealed class DeleteServiceHandler(
    IServiceRepository repo
)
{
    private readonly IServiceRepository _repo = repo;

    public async Task<Result> HandleAsync(
        DeleteServiceCommand cmd,
        CancellationToken ct
    )
    {
        var entity = await _repo.GetByIdAsync(cmd.Id, ct);
        if (entity is null) return Result.Failure("Service not found");

        await _repo.DeleteAsync(entity, ct);
        return Result.Success();
    }
}
