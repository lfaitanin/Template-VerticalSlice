using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using WebAPI.Features.Usuario.Commands;
using WebAPI.Features.Usuario.Handlers;
using WebAPI.Infrastructure.Repository;
using Xunit;

namespace WebAPI.Tests.Features.Usuario
{
    public class CreateUsersHandlerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly Mock<IPasswordHasher<WebAPI.Features.Usuario.Models.Usuario>> _mockPasswordHasher;
        private readonly Mock<IMediator> _mockMediator;
        private readonly CreateUsersHandler _handler;

        public CreateUsersHandlerTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<WebAPI.Features.Usuario.Models.Usuario>>();
            _mockMediator = new Mock<IMediator>();
            _handler = new CreateUsersHandler(_mockRepository.Object, _mockPasswordHasher.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new CreateUsersCommand
            {
                email = "existing@example.com",
                cpf = "12345678901",
                senha = "Password123!"
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByEmailAsync(command.email.ToLower()))
                           .ReturnsAsync(new WebAPI.Features.Usuario.Models.Usuario());

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("O e-mail já possui um cadastro.");
        }

        //[Fact]
        //public async Task Handle_ShouldThrowException_WhenCpfHasNoPreCadastro()
        //{
        //    // Arrange
        //    var command = new CreateUsersCommand
        //    {
        //        email = "new@example.com",
        //        cpf = "12345678901",
        //        senha = "Password123!"
        //    };

        //    _mockRepository.Setup(repo => repo.GetUsuarioByEmailAsync(command.email.ToLower()))
        //                   .ReturnsAsync((WebAPI.Features.Usuario.Models.Usuario)null);

        //    _mockRepository.Setup(repo => repo.GetUsuarioPreCadastroAsync(command.cpf))
        //                   .ReturnsAsync((WebAPI.Features.PreCadastro.Models.UsuarioPreCadastro)null);

        //    // Act
        //    Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        //    // Assert
        //    await act.Should().ThrowAsync<Exception>().WithMessage("O CPF informado não possui um pré-cadastro no APP.");
        //}

        [Fact]
        public async Task Handle_ShouldCreateUserSuccessfully_WhenValidDataIsProvided()
        {
            // Arrange
            var command = new CreateUsersCommand
            {
                email = "new@example.com",
                cpf = "12345678901",
                senha = "Password123!",
                nome = "User Name",
                foto = "photo_url",
                telefone = "123456789"
            };

            _mockRepository.Setup(repo => repo.GetUsuarioByEmailAsync(command.email.ToLower()))
                           .ReturnsAsync((WebAPI.Features.Usuario.Models.Usuario)null);

            _mockRepository.Setup(repo => repo.GetUsuarioPreCadastroAsync(command.cpf))
                           .ReturnsAsync(new List<WebAPI.Features.PreCadastro.Models.UsuarioPreCadastro>());

            _mockPasswordHasher.Setup(hasher => hasher.HashPassword(It.IsAny<WebAPI.Features.Usuario.Models.Usuario>(), command.senha))
                               .Returns("hashed_password");

            _mockRepository.Setup(repo => repo.AddUserAsync(It.IsAny<WebAPI.Features.Usuario.Models.Usuario>()))
                           .ReturnsAsync(Guid.NewGuid());

            _mockRepository.Setup(repo => repo.ConcluirPreCadastroAsync(command.cpf))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _mockRepository.Verify(repo => repo.AddUserAsync(It.IsAny<WebAPI.Features.Usuario.Models.Usuario>()), Times.Once);
            _mockRepository.Verify(repo => repo.ConcluirPreCadastroAsync(command.cpf), Times.Once);
        }
    }
}
