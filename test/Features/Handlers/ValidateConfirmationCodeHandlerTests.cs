using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using WebAPI.Features.Usuario.Commands;
using WebAPI.Features.Usuario.Handlers;
using WebAPI.Infrastructure.Repository;
using Xunit;

namespace WebAPI.Tests.Features.Usuario
{
    public class ValidateConfirmationCodeHandlerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly ValidateConfirmationCodeHandler _handler;

        public ValidateConfirmationCodeHandlerTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _handler = new ValidateConfirmationCodeHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var command = new ValidateConfirmationCodeCommand
            {
                CPF = "12345678901",
                Code = "123456"
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync((WebAPI.Features.Usuario.Models.Usuario)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Código inválido ou expirado.");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenConfirmationCodeIsInvalid()
        {
            // Arrange
            var command = new ValidateConfirmationCodeCommand
            {
                CPF = "12345678901",
                Code = "123456"
            };

            var user = new WebAPI.Features.Usuario.Models.Usuario
            {
                cpf = command.CPF,
                codigo_confirmacao = "654321", // Código armazenado diferente
                dt_expiracao_codigo = DateTime.UtcNow.AddMinutes(10) // Ainda válido
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Código inválido ou expirado.");
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenConfirmationCodeIsExpired()
        {
            // Arrange
            var command = new ValidateConfirmationCodeCommand
            {
                CPF = "12345678901",
                Code = "123456"
            };

            var user = new WebAPI.Features.Usuario.Models.Usuario
            {
                cpf = command.CPF,
                codigo_confirmacao = command.Code,
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
        public async Task Handle_ShouldReturnUnitValue_WhenConfirmationCodeIsValid()
        {
            // Arrange
            var command = new ValidateConfirmationCodeCommand
            {
                CPF = "12345678901",
                Code = "123456"
            };

            var user = new WebAPI.Features.Usuario.Models.Usuario
            {
                cpf = command.CPF,
                codigo_confirmacao = command.Code,
                dt_expiracao_codigo = DateTime.UtcNow.AddMinutes(10) // Código válido
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            _mockRepository.Verify(repo => repo.GetUsuarioByCPFAsync(command.CPF), Times.Once);
        }
    }
}
