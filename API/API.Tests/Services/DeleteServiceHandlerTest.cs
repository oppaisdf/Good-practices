using API.Application.Services.Contracts;
using API.Application.Services.Delete;
using API.Domain.Services;
using Moq;

namespace API.Tests.Services;

public sealed class DeleteServiceHandlerTests
{
    [Fact]
    public async Task HandleAsync_NotFound_Fails()
    {
        var repo = new Mock<IServiceRepository>(MockBehavior.Strict);
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Service?)null);

        var sut = new DeleteServiceHandler(repo.Object);

        var result = await sut.HandleAsync(new(Guid.NewGuid()), default);

        Assert.False(result.IsSuccess);
        Assert.Equal("Service not found", result.Error);
        repo.VerifyAll();
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleAsync_Existing_Deletes()
    {
        var entityResult = Service.Create("api", PortRange.Create(80, 90));
        var entity = entityResult.Value!;

        var repo = new Mock<IServiceRepository>(MockBehavior.Strict);
        repo.Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        repo.Setup(r => r.DeleteAsync(entity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new DeleteServiceHandler(repo.Object);

        var result = await sut.HandleAsync(new(entity.Id), default);

        Assert.True(result.IsSuccess);
        repo.VerifyAll();
        repo.VerifyNoOtherCalls();
    }
}
