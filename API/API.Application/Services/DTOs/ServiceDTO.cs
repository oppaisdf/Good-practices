namespace API.Application.Services.DTOs;

public sealed record ServiceDTO(
    Guid Id,
    string Name,
    ushort From,
    ushort To
);
