namespace API.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime Date { get; }
}
