using MediatR;
using WebAPI.Features.User.Models;

namespace WebAPI.Features.User.Queries
{
    public class ValidateUserQuery : IRequest<ValidaCadastroDto>
    {
        public string DocumentNumber { get; set; }
    }
} 