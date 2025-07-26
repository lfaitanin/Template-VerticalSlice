using MediatR;
using Shared.Domain.Entities.Response;

namespace WebAPI.Features.Perfil.Queries
{
    public class GetPerfisQuery : IRequest<BaseResponse>
    {
        public string cpf { get; set; }
    }
}
