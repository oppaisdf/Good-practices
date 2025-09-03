using API.Domain.Common;

namespace API.Domain.Services;

public sealed class Service
{
    private Service() { } // EF

    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public PortRange PortRange { get; private set; }

    // Fábrica de dominio: genera Guid aquí (no DB)
    public static Result<Service> Create(
        string name,
        PortRange portRange
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Service>.Failure("Name is required");

        var entity = new Service
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            PortRange = portRange
        };

        return Result<Service>.Success(entity);
    }

    public Result UpdateName(
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Name is required");
        Name = name.Trim();
        return Result.Success();
    }

    public Result UpdatePortRange(
        PortRange portRange
    )
    {
        PortRange = portRange;
        return Result.Success();
    }
}
