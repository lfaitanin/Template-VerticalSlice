using MediatR;
using WebAPI.Features.User.Commands;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.User.Handlers
{
    public class ValidateConfirmationCodeHandler : IRequestHandler<ValidateConfirmationCodeCommand, bool>
    {
        private readonly IUserRepository _repository;

        public ValidateConfirmationCodeHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ValidateConfirmationCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByDocumentNumberAsync(request.DocumentNumber);
            if (user == null)
            {
                return false;
            }

            if (user.confirmationCode != request.Code)
            {
                return false;
            }

            if (user.codeExpirationDate < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
} 