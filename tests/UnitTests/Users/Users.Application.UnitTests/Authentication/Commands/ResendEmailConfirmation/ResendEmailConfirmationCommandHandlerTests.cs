using ErrorOr;
using FluentAssertions;
using MediatR;
using Moq;
using Users.Application.Authentication.Command.ResendEmailConfirmation;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services.ConfirmationCodes;
using Users.Application.Common.Abstractions.Services.EmailNotifications;
using Users.Application.UnitTests.Common.TestUtils;
using Users.Domain;

namespace Users.Application.UnitTests.Authentication.Commands.ResendEmailConfirmation;

public class ResendEmailConfirmationCommandHandlerTests
{
    private readonly ResendEmailConfirmationCommandHandler _handler;
    private readonly Mock<ICodeRepository> _mockCodeRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IEmailNotification> _mockEmailNotification;
    private readonly Mock<IConfirmationCodeGenerator> _mockConfirmationCodeGenerator;

    public ResendEmailConfirmationCommandHandlerTests()
    {
        _mockCodeRepository = new Mock<ICodeRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockEmailNotification = new Mock<IEmailNotification>();
        _mockConfirmationCodeGenerator = new Mock<IConfirmationCodeGenerator>();

        _handler = new(
            _mockCodeRepository.Object,
            _mockUserRepository.Object,
            _mockEmailNotification.Object,
            _mockConfirmationCodeGenerator.Object);
    }

    [Fact]
    public async Task HandleResendEmailConfirmationCommand_WhenUserEmailIsValid_ShouldReturnUnit()
    {
        ResendEmailConfirmationCommand command = new("user@mail.com");
        User user = CreateUserUtils.CreateUser();
        Code code = CreateCodeUtils.CreateCode();

        _mockUserRepository.Setup(m => m.GetByEmail(command.UserEmail))
            .Returns(user);

        _mockCodeRepository.Setup(m => m.GetByUserId(user.Id))
            .Returns(code);

        _mockConfirmationCodeGenerator.Setup(m => m.GenerateConfirmationCode(It.IsAny<double>()))
            .Returns("123456");

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.Value.Should().BeOfType<Unit>();

        _mockUserRepository.Verify(m => m.GetByEmail(command.UserEmail), Times.Once);
        _mockCodeRepository.Verify(m => m.GetByUserId(user.Id), Times.Once);
        _mockConfirmationCodeGenerator.Verify(m => m.GenerateConfirmationCode(It.IsAny<double>()), Times.Once);
        _mockEmailNotification.Verify(m => m.SendNotificationAsync(It.IsAny<Message>()), Times.Once);
    }


    [Fact]
    public async Task HandleResendEmailConfirmationCommand_WhenUserDoesNotExist_ShouldReturnError()
    {
        ResendEmailConfirmationCommand command = new("user@mail.com");
        User user = CreateUserUtils.CreateUser();

        _mockUserRepository.Setup(m => m.GetByEmail(command.UserEmail))
            .Returns((User)null!);

        ErrorOr<Unit> result = await _handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("UserNotFound");

        _mockUserRepository.Verify(m => m.GetByEmail(command.UserEmail), Times.Once);
        _mockCodeRepository.Verify(m => m.GetByUserId(user.Id), Times.Never);
        _mockConfirmationCodeGenerator.Verify(m => m.GenerateConfirmationCode(It.IsAny<double>()), Times.Never);
        _mockEmailNotification.Verify(m => m.SendNotificationAsync(It.IsAny<Message>()), Times.Never);
    }
}