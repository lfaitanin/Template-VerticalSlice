using MediatR;

namespace WebAPI.Features.User.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string name { get; set; }
        public string email { get; set; }
        public string documentNumber { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string photo { get; set; }
        public List<int> profiles { get; set; }
    }
} 