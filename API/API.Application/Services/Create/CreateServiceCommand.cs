namespace API.Application.Services.Create;

public sealed record CreateServiceCommand(
    string Name,
    ushort From,
    ushort To
);
