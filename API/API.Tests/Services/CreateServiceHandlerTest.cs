using API.Application.Services.Contracts;
using API.Application.Services.Create;
using API.Application.Services.DTOs;
using API.Domain.Services;
using Moq;

namespace API.Tests.Services;

public sealed class CreateServiceHandlerTests
{
    [Fact]
    public async Task HandleAsync_Valid_CreatesService()
    {
        var repo = new Mock<IServiceRepository>(MockBehavior.Strict);
        repo.Setup(r => r.ExistsByNameAsync("api", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        repo.Setup(r => r.AddAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = new CreateServiceHandler(repo.Object);

        var result = await sut.HandleAsync(new("api", 80, 90), default);

        Assert.True(result.IsSuccess);
        Assert.IsType<ServiceDTO>(result.Value);
        Assert.Equal("api", result.Value!.Name);
        Assert.Equal((ushort)80, result.Value!.From);
        Assert.Equal((ushort)90, result.Value!.To);

        repo.VerifyAll();
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task HandleAsync_DuplicateName_Fails()
    {
        var repo = new Mock<IServiceRepository>(MockBehavior.Strict);
        repo.Setup(r => r.ExistsByNameAsync("api", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = new CreateServiceHandler(repo.Object);

        var result = await sut.HandleAsync(new("api", 80, 90), default);

        Assert.False(result.IsSuccess);
        Assert.Equal("A Service with the same name already exists", result.Error);

        repo.VerifyAll();
        repo.VerifyNoOtherCalls();
    }
}
