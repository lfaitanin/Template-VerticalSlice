using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Domain.Entities.Response;
using WebAPI.Features.ListaUsuarioPreCadastro.Commands;
using WebAPI.Features.Perfil.Models;
using WebAPI.Features.UsuarioPreCadastro.Handlers;
using WebAPI.Infrastructure.Repository;
using Xunit;

namespace WebAPI.Tests.Features.UsuarioPreCadastro
{
    public class CriarPerfilHandlerTests
    {
        private readonly Mock<IEntubaRepository> _mockRepository;
        private readonly Mock<ILogger<CriarPerfilHandler>> _mockLogger;
        private readonly CriarPerfilHandler _handler;

        public CriarPerfilHandlerTests()
        {
            _mockRepository = new Mock<IEntubaRepository>();
            _mockLogger = new Mock<ILogger<CriarPerfilHandler>>();
            _handler = new CriarPerfilHandler(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenProfilesAreCreated()
        {
            // Arrange
            var perfil = new Perfil() { descricao = "perfil1", id_perfil = 1, status = "Ativo" };
            var command = new CriarPerfilCommand([perfil]);

            _mockRepository
                .Setup(repo => repo.AddPerfilAsync(It.IsAny<Perfil>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Sucesso");

            // Verificar se os perfis foram adicionados corretamente
            _mockRepository.Verify(repo => repo.AddPerfilAsync(perfil), Times.Once);

            // Verificar logs
            _mockLogger.Verify(logger => logger.LogInformation("Inicio criar Perfil"), Times.Once);
            _mockLogger.Verify(logger => logger.LogInformation("Fim criar Perfil"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenRepositoryThrowsException()
        {
            // Arrange
            var perfil = new Perfil() { descricao = "perfil1", id_perfil = 1, status = "Ativo" };
            var command = new CriarPerfilCommand([perfil]);

            _mockRepository
                .Setup(repo => repo.AddPerfilAsync(It.IsAny<Perfil>()))
                .ThrowsAsync(new Exception("Erro ao adicionar perfil"));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Falha");

            // Verificar logs de erro
            _mockLogger.Verify(logger => logger.LogError("Fim criar Perfil"), Times.Once);
        }
    }
}
