using MediatR;
using Shared.Services;
using WebAPI.Features.User.Commands;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.User.Handlers
{
    public class SendConfirmationCodeHandler : IRequestHandler<SendConfirmationCodeCommand, bool>
    {
        private readonly IUserRepository _repository;
        private readonly IEmailService _emailService;

        public SendConfirmationCodeHandler(IUserRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<bool> Handle(SendConfirmationCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByDocumentNumberAsync(request.DocumentNumber);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var code = GenerateConfirmationCode();
            user.confirmationCode = code;
            user.codeExpirationDate = DateTime.UtcNow.AddMinutes(10);

            await _repository.UpdateUserAsync(user);

            // Send email with confirmation code
            await _emailService.SendEmailAsync(
                user.email,
                "Password Reset Confirmation Code",
                $"Your confirmation code is: {code}. This code expires in 10 minutes."
            );

            return true;
        }

        private string GenerateConfirmationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
} 