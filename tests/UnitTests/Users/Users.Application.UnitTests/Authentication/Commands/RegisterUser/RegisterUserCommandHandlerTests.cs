using ErrorOr;
using FluentAssertions;
using Moq;
using Users.Application.Authentication.Command.RegisterUser;
using Users.Application.Authentication.Common;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Application.Common.Abstractions.Services.ConfirmationCodes;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Application.UnitTests.Authentication.Commands.RegisterUser.TestUtils;
using Users.Domain;

namespace Users.Application.UnitTests.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly RegisterUserCommandHandler _handler;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtTokenGenerator> _mockJwtGenerator;
    private readonly Mock<IEmailNotification> _mockEmailConfirmation;
    private readonly Mock<IConfirmationCodeGenerator> _mockConfirmationCodeGenerator;
    private readonly Mock<ICodeRepository> _mockCodeRepository;

    public RegisterUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtGenerator = new Mock<IJwtTokenGenerator>();
        _mockEmailConfirmation = new Mock<IEmailNotification>();
        _mockConfirmationCodeGenerator = new Mock<IConfirmationCodeGenerator>();
        _mockCodeRepository = new Mock<ICodeRepository>();

        _handler = new(
            _mockUserRepository.Object,
            _mockJwtGenerator.Object,
            _mockEmailConfirmation.Object,
            _mockConfirmationCodeGenerator.Object,
            _mockCodeRepository.Object);
    }

    [Fact]
    public async Task HandleRegisterUserCommand_WhenUserIsValid_ShouldCreateAndReturnUseId_Email_TokenAsync()
    {
        RegisterUserCommand command = RegisterUserCommandUtils.CreateRegisterCommand();
        string token = RegisterUserCommandUtils.GenerateToken();
        string code = RegisterUserCommandUtils.GenerateCode();

        _mockJwtGenerator.Setup(m => m.GenerateUserToken(It.IsAny<User>()))
            .Returns(token);

        _mockConfirmationCodeGenerator.Setup(m => m.GenerateConfirmationCode(It.IsAny<double>()))
            .Returns(code);

        ErrorOr<AuthenticationResult> result = await _handler.Handle(command, default);

        result.Value.Should().BeOfType<AuthenticationResult>();
        result.Value.Email.Should().Be(command.Email);
        result.Value.Token.Should().Be(token);

        _mockUserRepository.Verify(m => m.GetByEmail(It.IsAny<string>()), Times.Once);
        _mockUserRepository.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
        _mockJwtGenerator.Verify(m => m.GenerateUserToken(It.IsAny<User>()), Times.Once);
        _mockEmailConfirmation.Verify(m => m.SendNotification(It.IsAny<Message>()), Times.Once);
        _mockCodeRepository.Verify(m => m.Add(It.IsAny<Code>()), Times.Once);
        _mockConfirmationCodeGenerator.Verify(m => m.GenerateConfirmationCode(It.IsAny<double>()), Times.Once);
    }

    [Fact]
    public async Task HandleRegisterUserCommand_WhenUserEmailExists_ShouldNotCreateAndReturnError()
    {
        RegisterUserCommand command = RegisterUserCommandUtils.CreateRegisterCommand();
        string token = RegisterUserCommandUtils.GenerateToken();
        string code = RegisterUserCommandUtils.GenerateCode();

        _mockUserRepository.Setup(m => m.GetByEmail(command.Email))
            .Returns(User.Create(
                command.FirstName,
                command.LastName,
                command.Email,
                command.Password));

        _mockConfirmationCodeGenerator.Setup(m => m.GenerateConfirmationCode(It.IsAny<double>()))
            .Returns(code);

        ErrorOr<AuthenticationResult> result = await _handler.Handle(command, default);

        result.Value.Should().BeNull();
        result.FirstError.Code.Should().Be("EmailExists");
        result.FirstError.Type.Should().Be(ErrorType.Conflict);

        _mockUserRepository.Verify(m => m.GetByEmail(It.IsAny<string>()), Times.Once);
        _mockUserRepository.Verify(m => m.Add(It.IsAny<User>()), Times.Never);
        _mockJwtGenerator.Verify(m => m.GenerateUserToken(It.IsAny<User>()), Times.Never);
        _mockEmailConfirmation.Verify(m => m.SendNotification(It.IsAny<Message>()), Times.Never);
        _mockCodeRepository.Verify(m => m.Add(It.IsAny<Code>()), Times.Never);
        _mockConfirmationCodeGenerator.Verify(m => m.GenerateConfirmationCode(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task HandleRegisterUserCommand_WhenEmailNotificationIsDown_ShouldNotCreateAndReturnError()
    {
        RegisterUserCommand command = RegisterUserCommandUtils.CreateRegisterCommand();
        string token = RegisterUserCommandUtils.GenerateToken();
        string code = RegisterUserCommandUtils.GenerateCode();

        _mockJwtGenerator.Setup(m => m.GenerateUserToken(It.IsAny<User>()))
            .Returns(token);

        _mockConfirmationCodeGenerator.Setup(m => m.GenerateConfirmationCode(It.IsAny<double>()))
            .Returns(code);

        _mockEmailConfirmation.Setup(m => m.SendNotification(It.IsAny<Message>()))
            .Throws<Exception>();

        ErrorOr<AuthenticationResult> result = await _handler.Handle(command, default);

        result.Value.Should().BeOfType<AuthenticationResult>();
        result.Value.Email.Should().Be(command.Email);
        result.Value.Token.Should().Be(token);

        _mockUserRepository.Verify(m => m.GetByEmail(It.IsAny<string>()), Times.Once);
        _mockUserRepository.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
        _mockJwtGenerator.Verify(m => m.GenerateUserToken(It.IsAny<User>()), Times.Once);
        _mockEmailConfirmation.Verify(m => m.SendNotification(It.IsAny<Message>()), Times.Once);
        _mockCodeRepository.Verify(m => m.Add(It.IsAny<Code>()), Times.Once);
        _mockConfirmationCodeGenerator.Verify(m => m.GenerateConfirmationCode(It.IsAny<double>()), Times.Once);
    }
}
