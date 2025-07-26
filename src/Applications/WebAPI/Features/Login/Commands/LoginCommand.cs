using MediatR;

namespace WebAPI.Features.Login.Commands
{
    public class LoginCommand : IRequest<(string token, string userId)>
    {
        public string EmailOrDocumentNumber { get; set; }
        public string Password { get; set; }
    }
}
