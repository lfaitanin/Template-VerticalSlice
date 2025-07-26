using MediatR;
using WebAPI.Features.User.Queries;
using UserModel = WebAPI.Features.User.Models.User;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.User.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserModel>>
    {
        private readonly IUserRepository _repository;

        public GetUsersHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllUsersAsync();
        }
    }
} 