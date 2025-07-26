using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Shared.Services;
using WebAPI.Features.Usuario.Commands;
using WebAPI.Features.Usuario.Handlers;
using WebAPI.Infrastructure.Repository;
using Xunit;

namespace WebAPI.Tests.Features.Usuario
{
    public class SendConfirmationCodeHandlerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly SendConfirmationCodeHandler _handler;

        public SendConfirmationCodeHandlerTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _handler = new SendConfirmationCodeHandler(_mockRepository.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserIsNotFound()
        {
            // Arrange
            var command = new SendConfirmationCodeCommand
            {
                CPF = "12345678901"
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync((WebAPI.Features.Usuario.Models.Usuario)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Usuário não encontrado.");
        }

        [Fact]
        public async Task Handle_ShouldGenerateAndSaveConfirmationCode_WhenUserIsFound()
        {
            // Arrange
            var command = new SendConfirmationCodeCommand
            {
                CPF = "12345678901"
            };

            var user = new WebAPI.Features.Usuario.Models.Usuario
            {
                cpf = command.CPF,
                email = "user@example.com",
                tentativas = 1,
                bloqueado = true
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync(user);

            _mockRepository.Setup(repo => repo.AtualizarUsuarioAsync(It.IsAny<WebAPI.Features.Usuario.Models.Usuario>()))
                           .Returns(Task.CompletedTask);

            _mockEmailService.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(Task.CompletedTask);

            // Act
            var maskedEmail = await _handler.Handle(command, CancellationToken.None);

            // Assert
            maskedEmail.Should().Be("u*****e@example.com"); // Verifique o formato esperado do e-mail mascarado

            user.codigo_confirmacao.Should().NotBeNullOrEmpty();
            user.dt_expiracao_codigo.Should().BeAfter(DateTime.UtcNow);
            user.bloqueado.Should().BeFalse();
            user.tentativas.Should().Be(0);

            _mockRepository.Verify(repo => repo.AtualizarUsuarioAsync(It.Is<WebAPI.Features.Usuario.Models.Usuario>(u => u.codigo_confirmacao == user.codigo_confirmacao)), Times.Once);
            _mockEmailService.Verify(service => service.SendEmailAsync("user@example.com", "Código de Confirmação", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldSendEmailWithCorrectDetails()
        {
            // Arrange
            var command = new SendConfirmationCodeCommand
            {
                CPF = "12345678901"
            };

            var user = new WebAPI.Features.Usuario.Models.Usuario
            {
                cpf = command.CPF,
                email = "user@example.com"
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByCPFAsync(command.CPF))
                           .ReturnsAsync(user);

            _mockEmailService.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockEmailService.Verify(service => service.SendEmailAsync(
                "user@example.com",
                "Código de Confirmação",
                It.Is<string>(body => body.Contains("Seu código de confirmação é:"))
            ), Times.Once);
        }
    }
}
