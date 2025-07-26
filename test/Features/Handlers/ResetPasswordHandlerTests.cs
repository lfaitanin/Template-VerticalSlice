using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Shared.Services;
using WebAPI.Features.Usuario.Commands;
using WebAPI.Features.Usuario.Handlers;
using WebAPI.Infrastructure.Repository;
using Xunit;

namespace WebAPI.Tests.Features.Usuario
{
    public class ResetPasswordHandlerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly ResetPasswordHandler _handler;

        public ResetPasswordHandlerTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _handler = new ResetPasswordHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserIsNotFound()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                CPF = "12345678901",
                Code = "123456",
                NewPassword = "NewPassword123!"
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync((WebAPI.Features.Usuario.Models.Usuario)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Código inválido ou expirado.");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenConfirmationCodeIsInvalidOrExpired()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                CPF = "12345678901",
                Code = "123456",
                NewPassword = "NewPassword123!"
            };

            var user = new WebAPI.Features.Usuario.Models.Usuario
            {
                cpf = command.CPF,
                codigo_confirmacao = "654321", // Código diferente
                dt_expiracao_codigo = DateTime.UtcNow.AddMinutes(-1) // Código expirado
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Código inválido ou expirado.");
        }

        [Fact]
        public async Task Handle_ShouldResetPasswordSuccessfully_WhenAllCriteriaAreMet()
        {
            // Arrange
            var command = new ResetPasswordCommand
            {
                CPF = "12345678901",
                Code = "123456",
                NewPassword = "NewPassword123!"
            };

            var user = new WebAPI.Features.Usuario.Models.Usuario
            {
                cpf = command.CPF,
                codigo_confirmacao = command.Code,
                dt_expiracao_codigo = DateTime.UtcNow.AddMinutes(10), // Código válido
                senha = "OldPasswordHash"
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync(user);

            _mockRepository.Setup(repo => repo.AtualizarUsuarioAsync(It.IsAny<WebAPI.Features.Usuario.Models.Usuario>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            // Verificar se o usuário foi atualizado corretamente
            user.senha.Should().NotBe("OldPasswordHash");
            user.codigo_confirmacao.Should().BeNull();
            user.dt_expiracao_codigo.Should().BeNull();

            _mockRepository.Verify(repo => repo.AtualizarUsuarioAsync(It.IsAny<WebAPI.Features.Usuario.Models.Usuario>()), Times.Once);
        }
    }
}
