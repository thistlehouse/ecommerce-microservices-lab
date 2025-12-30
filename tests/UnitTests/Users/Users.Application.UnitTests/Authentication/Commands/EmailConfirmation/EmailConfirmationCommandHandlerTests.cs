using ErrorOr;
using FluentAssertions;
using MediatR;
using Moq;
using Users.Application.Authentication.Command.EmailConfirmation;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.UnitTests.Common.TestUtils;
using Users.Domain;

namespace Users.Application.UnitTests.Authentication.Commands.EmailConfirmation;

public sealed class EmailConfirmationCommandHandlerTests
{
    private readonly EmailConfirmationCommandHandler _handler;
    private readonly Mock<ICodeRepository> _mockCodeRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;

    public EmailConfirmationCommandHandlerTests()
    {
        _mockCodeRepository = new Mock<ICodeRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new(_mockCodeRepository.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task HandleEmailConfirmationCommand_WhenCodeExists_ShouldReturnUnit()
    {
        EmailConfirmationCommand command = new("123456");
        User user = CreateUserUtils.CreateUser();
        Code code = CreateCodeUtils.CreateCode(userId: user.Id);

        _mockCodeRepository.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(code);

        _mockUserRepository.Setup(m => m.GetById(It.IsAny<Guid>()))
            .Returns(user);

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.Value.Should().BeOfType<Unit>();

        _mockUserRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Once);
        _mockCodeRepository.Verify(m => m.Get(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task HandleEmailConfirmationCommand_WhenCodeDoesNotExists_ShouldReturnError()
    {
        EmailConfirmationCommand command = new("123456");

        _mockCodeRepository.Setup(m => m.Get(It.IsAny<string>()))
            .Returns((Code)null!);

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.FirstError.Code.Should().Be("InvalidCode");

        _mockUserRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Never);
        _mockCodeRepository.Verify(m => m.Get(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task HandleEmailConfirmationCommand_WhenCodeHasExpired_ShouldReturnError()
    {
        EmailConfirmationCommand command = new("123456");
        Code code = CreateCodeUtils.CreateCode(expiresInMinutes: -5);

        _mockCodeRepository.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(code);

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.FirstError.Code.Should().Be("CodeExpired");

        _mockUserRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Never);
        _mockCodeRepository.Verify(m => m.Get(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task HandleEmailConfirmationCommand_WhenUsedAtHasValue_ShouldReturnError()
    {
        EmailConfirmationCommand command = new("123456");
        Code code = CreateCodeUtils.CreateCode(expiresInMinutes: -5);
        code.MarkAsUsed(DateTime.Now);

        _mockCodeRepository.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(code);

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.FirstError.Code.Should().Be("CodeExpired");

        _mockUserRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Never);
        _mockCodeRepository.Verify(m => m.Get(It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task HandleEmailConfirmationCommand_WhenUserDoesNotExists_ShouldReturnError()
    {
        EmailConfirmationCommand command = new("123456");
        User user = CreateUserUtils.CreateUser();
        Code code = CreateCodeUtils.CreateCode();

        _mockCodeRepository.Setup(m => m.Get(It.IsAny<string>()))
            .Returns(code);

        _mockUserRepository.Setup(m => m.GetById(It.IsAny<Guid>()))
            .Returns((User)null!);

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.FirstError.Code.Should().Be("UserNotFound");

        _mockUserRepository.Verify(m => m.GetById(It.IsAny<Guid>()), Times.Once);
        _mockCodeRepository.Verify(m => m.Get(It.IsAny<string>()), Times.Once);
    }
}