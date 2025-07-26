using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using WebAPI.Features.PreCadastro.Commands;
using WebAPI.Features.PreCadastro.Handlers;
using WebAPI.Infrastructure.Repository;
using Xunit;

namespace WebAPI.Tests.Features.PreCadastro
{
    public class ImportarUsuariosPreCadastroHandlerTests
    {
        private readonly Mock<IEntubaRepository> _mockRepository;
        private readonly Mock<ILogger<ImportarUsuariosPreCadastroHandler>> _mockLogger;
        private readonly ImportarUsuariosPreCadastroHandler _handler;

        public ImportarUsuariosPreCadastroHandlerTests()
        {
            _mockRepository = new Mock<IEntubaRepository>();
            _mockLogger = new Mock<ILogger<ImportarUsuariosPreCadastroHandler>>();
            _handler = new ImportarUsuariosPreCadastroHandler(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenUsersAreImportedSuccessfully()
        {
            // Arrange
            var fileContent = CreateExcelFile(new List<string[]>
            {
                new[] { "12345678901", "User One", "Gestor" },
                new[] { "98765432100", "User Two", "Professor" }
            });

            var command = new ImportarUsuariosPreCadastroCommand(new MockFormFile(fileContent, "test.xlsx"));

            _mockRepository.Setup(repo => repo.Exists(It.IsAny<string>())).ReturnsAsync(false);
            _mockRepository.Setup(repo => repo.AddRangeAsync(It.IsAny<IEnumerable<WebAPI.Features.PreCadastro.Models.UsuarioPreCadastro>>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Message.Should().Be("Sucesso");
            response.Result.Should().BeOfType<List<WebAPI.Features.PreCadastro.Models.UsuarioPreCadastro>>();

            _mockLogger.Verify(logger => logger.LogInformation("Inicio entuba PreCadastro"), Times.Exactly(2));
            _mockLogger.Verify(logger => logger.LogInformation("Fim entuba PreCadastro"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResponse_WhenInvalidDataIsProvided()
        {
            // Arrange
            var fileContent = CreateExcelFile(new List<string[]>
            {
                new[] { "12345678901", "User One", "InvalidPerfil" }
            });

            var command = new ImportarUsuariosPreCadastroCommand(new MockFormFile(fileContent, "test.xlsx"));

            _mockRepository.Setup(repo => repo.Exists(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Falha no entuba");
            response.Result.Should().BeOfType<List<WebAPI.Features.PreCadastro.Models.UsuarioPreCadastro>>();

            _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Erro: {cpf}", "12345678901"), Times.Once);
        }

        private Stream CreateExcelFile(List<string[]> rows)
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream, Encoding.UTF8);

            // Simula conteúdo CSV (pode ser ajustado para ExcelDataReader)
            foreach (var row in rows)
            {
                writer.WriteLine(string.Join(",", row));
            }

            writer.Flush();
            memoryStream.Position = 0;

            return memoryStream;
        }

        private class MockFormFile : Microsoft.AspNetCore.Http.IFormFile
        {
            private readonly Stream _content;

            public MockFormFile(Stream content, string fileName)
            {
                _content = content;
                FileName = fileName;
            }

            public string ContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{FileName}\"";
            public IHeaderDictionary Headers => new HeaderDictionary();
            public long Length => _content.Length;
            public string Name => "file";
            public string FileName { get; }

            public void CopyTo(Stream target) => _content.CopyTo(target);
            public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => _content.CopyToAsync(target, cancellationToken);
            public Stream OpenReadStream() => _content;
        }
    }
}
