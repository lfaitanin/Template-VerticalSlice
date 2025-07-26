using Moq;
using WebAPI.Features.Users.Handlers;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Tests.Features.Users
{
    public class ValidaUsuarioHandlerTests
    {
        private readonly Mock<IEntubaRepository> _mockRepository;
        private readonly ValidaUsuarioHandler _handler;

        public ValidaUsuarioHandlerTests()
        {
            _mockRepository = new Mock<IEntubaRepository>();
            _handler = new ValidaUsuarioHandler(_mockRepository.Object);
        }

    }
}
