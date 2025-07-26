using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Services;
using WebAPI.Features.User.Commands;
using UserModel = WebAPI.Features.User.Models.User;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.User.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userDb = await _userRepository.GetUserByEmailAsync(request.email.ToLower());
            if (userDb is not null)
            {
                throw new Exception("This email is already being used.");
            }

            List<PreRegistration.Models.UserPreRegistration> usersPreRegistration = await _userRepository.GetUserPreRegistrationAsync(request.documentNumber);
            if (usersPreRegistration is null || usersPreRegistration?.Count == 0)
            {
                throw new Exception("The provided document number does not have a pre-registration in the APP.");
            }
            
            var user = new UserModel
            {
                id = Guid.NewGuid(),
                name = request.name,
                email = request.email,
                documentNumber = request.documentNumber,
                photo = request.photo,
                phone = request.phone,
            };
            user.password = PasswordService.HashPassword(request.password);
            var idUser = await _userRepository.AddUserAsync(user);

            foreach (var profile in request.profiles)
            {
                var userProfile = new UserProfile
                {
                    ProfileId = profile,
                    UserId = user.id
                };
                await _userRepository.InsertUserProfile(userProfile);
            }

            await _userRepository.CompletePreRegistrationAsync(user.documentNumber);
            return user.id;
        }
    }
} 