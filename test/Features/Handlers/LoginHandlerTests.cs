using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebAPI.Features.Login.Commands;
using WebAPI.Features.Login.Handlers;
using WebAPI.Infrastructure.Repository;
using WebAPI.Features.Usuario.Models;
using Shared.Services;
using Moq;
using Xunit;
using System.IdentityModel.Tokens.Jwt;

namespace Test;
public class LoginHandlerTests
{
    private readonly Mock<IUsuarioRepository> _mockRepository;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        // Mock do repositório de usuário
        _mockRepository = new Mock<IUsuarioRepository>();

        // Mock da configuração
        _mockConfig = new Mock<IConfiguration>();
        var mockJwtSettings = new Mock<IConfigurationSection>();
        mockJwtSettings.Setup(x => x["Key"]).Returns("SuperSecretKeyForJwt");
        mockJwtSettings.Setup(x => x["Issuer"]).Returns("TestIssuer");
        mockJwtSettings.Setup(x => x["Audience"]).Returns("TestAudience");
        mockJwtSettings.Setup(x => x["ExpireMinutes"]).Returns("30");

        _mockConfig.Setup(x => x.GetSection("Jwt")).Returns(mockJwtSettings.Object);

        // Instância do handler com mocks
        _handler = new LoginHandler(_mockRepository.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenUserAuthenticatesSuccessfully()
    {
        // Arrange
        var command = new LoginCommand(new WebAPI.Features.Login.Models.LoginModel
        {
            EmailOuCPF = "testuser@example.com",
            Senha = "ValidPassword"
        });

        var user = new Usuario
        {
            id_usuario = Guid.NewGuid(),
            email = "testuser@example.com",
            senha = PasswordService.HashPassword("ValidPassword"), // Simula uma senha válida
            tentativas = 0,
            bloqueado = false
        };

        _mockRepository.Setup(r => r.GetUsuarioByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var handler = new JwtSecurityTokenHandler();
        Assert.True(handler.CanReadToken(result.token)); // Verifica se o token é legível
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginCommand(new WebAPI.Features.Login.Models.LoginModel()
        {
            EmailOuCPF = "testuser@example.com",
            Senha = "WrongPassword"
        });

        var user = new Usuario
        {
            id_usuario = Guid.NewGuid(),
            email = "testuser@example.com",
            senha = PasswordService.HashPassword("ValidPassword"),
            tentativas = 0,
            bloqueado = false
        };

        _mockRepository.Setup(r => r.GetUsuarioByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Senha incorreta! Número de tentativas 1/3", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserIsBlocked()
    {
        // Arrange
        var command = new LoginCommand(new WebAPI.Features.Login.Models.LoginModel
        {
            EmailOuCPF = "testuser@example.com",
            Senha = "ValidPassword"
        });
        
        var user = new Usuario
        {
            id_usuario = Guid.NewGuid(),
            email = "testuser@example.com",
            senha = PasswordService.HashPassword("ValidPassword"),
            tentativas = 3,
            bloqueado = true
        };

        _mockRepository.Setup(r => r.GetUsuarioByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Usuário bloqueado. Resete a sua senha.", exception.Message);
    }
}
