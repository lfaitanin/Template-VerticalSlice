using MediatR;
using UserModel = WebAPI.Features.User.Models.User;

namespace WebAPI.Features.User.Queries
{
    public class GetUsersQuery : IRequest<List<UserModel>>
    {
    }
} 