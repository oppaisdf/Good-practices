namespace API.Domain.Services;

/// <summary>
/// Value Object: rango de puertos [From, To].
/// - Inmutable.
/// - Valida que From <= To.
/// </summary>
public sealed record class PortRange
{
    public ushort From { get; }
    public ushort To { get; }

    // Necesario para EF Core (materialización).
    public PortRange() { }

    // Constructor canónico con validación.
    public PortRange(ushort from, ushort to)
    {
        if (from > to) throw new ArgumentException("'from' must be <= 'to'");
        From = from;
        To = to;
    }

    // Fábrica alternativa para legibilidad.
    public static PortRange Create(
        ushort from,
        ushort to
    ) => new(from, to);
    public bool Contains(
        ushort port
    ) => port >= From && port <= To;
    public override string ToString() => $"{From}-{To}";
}
