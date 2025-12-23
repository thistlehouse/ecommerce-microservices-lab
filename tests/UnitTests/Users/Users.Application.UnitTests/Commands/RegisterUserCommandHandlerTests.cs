using ErrorOr;
using FluentAssertions;
using Moq;
using Users.Application.Authentication.Command.RegisterUser;
using Users.Application.Authentication.Common;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Domain;

namespace Users.Application.UnitTests.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly RegisterUserCommandHandler _handler;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtTokenGenerator> _mockJwtGenerator;

    public RegisterUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtGenerator = new Mock<IJwtTokenGenerator>();
        _handler = new(_mockUserRepository.Object, _mockJwtGenerator.Object);
    }

    [Fact]
    public async Task HandleRegisterUserCommand_WhenUserIsValid_ShouldCreateAndReturnUseId_Email_TokenAsync()
    {
        RegisterUserCommand command = new(
            "UserFistName",
            "UserLastName",
            "user@mail.com",
            "password");

        string token = "token.token.token";

        _mockJwtGenerator.Setup(m => m.GenerateToken(It.IsAny<User>()))
            .Returns(token);

        ErrorOr<AuthenticationResult> result = await _handler.Handle(command, default);

        result.Value.Should().BeOfType<AuthenticationResult>();
        result.Value.Email.Should().Be(command.Email);
        result.Value.Token.Should().Be(token);

        _mockUserRepository.Verify(m => m.Create(It.IsAny<User>()), Times.Once);
        _mockJwtGenerator.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task HandleRegisterUserCommand_WhenUserEmailExists_ShouldNotCreateAndReturnError()
    {
        RegisterUserCommand command = new(
            "UserFistName",
            "UserLastName",
            "user@mail.com",
            "password");

        _mockUserRepository.Setup(m => m.GetByEmail(command.Email))
            .Returns(User.Create(
                command.FirstName,
                command.LastName,
                command.Email,
                command.Password));

        ErrorOr<AuthenticationResult> result = await _handler.Handle(command, default);

        result.Value.Should().BeNull();
        result.FirstError.Code.Should().Be("EmailExists");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);

        _mockUserRepository.Verify(m => m.Create(It.IsAny<User>()), Times.Never);
        _mockJwtGenerator.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}
