using MediatR;
using Shared.Services;
using WebAPI.Features.User.Commands;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.User.Handlers
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _repository;

        public ResetPasswordHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByDocumentNumberAsync(request.DocumentNumber);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (user.confirmationCode != request.Code)
            {
                throw new Exception("Invalid confirmation code.");
            }

            if (user.codeExpirationDate < DateTime.UtcNow)
            {
                throw new Exception("Confirmation code has expired.");
            }

            user.password = PasswordService.HashPassword(request.NewPassword);
            user.confirmationCode = null;
            user.codeExpirationDate = null;
            user.blocked = false;
            user.attempts = 0;

            await _repository.UpdateUserAsync(user);
            return true;
        }
    }
} 