using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WebAPI.Features.Users.Handlers;
using WebAPI.Features.Users.Queries;
using WebAPI.Infrastructure.Repository;
using Xunit;

namespace WebAPI.Tests.Features.Users
{
    public class GetUsersHandlerTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly GetUsersHandler _handler;

        public GetUsersHandlerTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _handler = new GetUsersHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfUsers_WhenUsersExist()
        {
            // Arrange
            var expectedUsers = new List<WebAPI.Features.Usuario.Models.Usuario>
            {
                new WebAPI.Features.Usuario.Models.Usuario { id_usuario = Guid.NewGuid(), nome = "User One" },
                new WebAPI.Features.Usuario.Models.Usuario { id_usuario = Guid.NewGuid(), nome = "User Two" }
            };

            _mockRepository.Setup(repo => repo.GetAllUsuariosAsync())
                           .ReturnsAsync(expectedUsers);

            var query = new GetUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedUsers);

            // Verifica se o método GetAllUsuariosAsync foi chamado uma vez
            _mockRepository.Verify(repo => repo.GetAllUsuariosAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllUsuariosAsync())
                           .ReturnsAsync(new List<WebAPI.Features.Usuario.Models.Usuario>());

            var query = new GetUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            // Verifica se o método GetAllUsuariosAsync foi chamado uma vez
            _mockRepository.Verify(repo => repo.GetAllUsuariosAsync(), Times.Once);
        }
    }
}
