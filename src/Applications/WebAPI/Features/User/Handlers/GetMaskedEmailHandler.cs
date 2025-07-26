using MediatR;
using WebAPI.Features.User.Commands;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.User.Handlers
{
    public class GetMaskedEmailHandler : IRequestHandler<GetMaskedEmailCommand, string>
    {
        private readonly IUserRepository _repository;

        public GetMaskedEmailHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(GetMaskedEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByDocumentNumberAsync(request.DocumentNumber);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var email = user.email;
            var atIndex = email.IndexOf('@');
            if (atIndex <= 1)
            {
                return email;
            }

            var username = email.Substring(0, atIndex);
            var domain = email.Substring(atIndex);
            var maskedUsername = username.Substring(0, 1) + new string('*', username.Length - 2) + username.Substring(username.Length - 1);

            return maskedUsername + domain;
        }
    }
} 