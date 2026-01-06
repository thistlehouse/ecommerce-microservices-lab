using ErrorOr;
using FluentAssertions;
using Moq;
using Users.Application.Authentication.Common;
using Users.Application.Authentication.Queries;
using Users.Application.Common.Abstractions.Repositories;
using Users.Application.Common.Abstractions.Services;
using Users.Application.UnitTests.Common.TestUtils;
using Users.Domain;

namespace Users.Application.UnitTests.Queries;

public class LoginUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtTokenGenerator> _mockJwtGenerator;
    private readonly LoginUserQueryHandler _handler;

    public LoginUserQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtGenerator = new Mock<IJwtTokenGenerator>();
        _handler = new(_mockUserRepository.Object, _mockJwtGenerator.Object);
    }

    [Fact]
    public async Task HandleLoginUserQuery_WhenUserExist_AndPasswordMatches_ShouldReturnUserId_Email_Token()
    {
        LoginUserQuery query = new("user@mail.com", "password");

        _mockUserRepository.Setup(m => m.GetByEmail(query.Email))
            .Returns(CreateUserUtils.CreateUser());

        string token = "token.token.token";
        _mockJwtGenerator.Setup(m => m.GenerateToken(It.IsAny<User>()))
            .Returns(token);

        ErrorOr<AuthenticationResult> result = await _handler.Handle(query, default);

        result.Value.Should().BeOfType<AuthenticationResult>();
        result.Value.Email.Should().Be(query.Email);
        result.Value.Token.Should().Be(token);

        _mockUserRepository.Verify(m => m.GetByEmail(query.Email), Times.Once);
        _mockJwtGenerator.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task HandleLoginUserQuery_WhenUserDoesNotExist_ShouldReturnError()
    {
        LoginUserQuery query = new("user@mail.com", "password");

        _mockUserRepository.Setup(m => m.GetByEmail(query.Email))
            .Returns((User)null!);

        ErrorOr<AuthenticationResult> result = await _handler.Handle(query, default);

        result.Value.Should().BeNull();
        result.FirstError.Code.Should().Be("UserNotFound");

        _mockUserRepository.Verify(m => m.GetByEmail(query.Email), Times.Once);
        _mockJwtGenerator.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task HandleLoginUserQuery_WhenUserExists_AndPasswordDoesNotMatch_ShouldReturnError()
    {
        LoginUserQuery query = new("user@mail.com", "passw0rd");

        _mockUserRepository.Setup(m => m.GetByEmail(query.Email))
            .Returns(CreateUserUtils.CreateUser());

        ErrorOr<AuthenticationResult> result = await _handler.Handle(query, default);

        result.Value.Should().BeNull();
        result.FirstError.Code.Should().Be("InvalidCredentials");

        _mockUserRepository.Verify(m => m.GetByEmail(query.Email), Times.Once);
        _mockJwtGenerator.Verify(m => m.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}