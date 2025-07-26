using MediatR;
using Microsoft.IdentityModel.Tokens;
using Shared.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using WebAPI.Features.Login.Commands;
using UserModel = WebAPI.Features.User.Models.User;
using WebAPI.Infrastructure.Repository;

namespace WebAPI.Features.Login.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, (string token, string userId)>
    {
        private readonly IUserRepository _repository;
        private readonly SigningCredentials _credentials;
        private readonly IConfigurationSection _jwtSettings;

        public LoginHandler(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;

            _jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings["Key"]));
            _credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public async Task<(string token, string userId)> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            bool isDocumentNumber = IsDocumentNumber(request.EmailOrDocumentNumber);
            var user = await AuthenticateUser(request, isDocumentNumber);

            if (user != null)
            {
                var token = GenerateJwtToken(user);
                return (token, user.id.ToString());
            }

            return default;
        }

        private bool IsDocumentNumber(string input)
        {
            input = input.Trim();
            return input.Length == 11 && long.TryParse(input, out _);
        }

        private async Task<UserModel> AuthenticateUser(LoginCommand request, bool isDocumentNumber)
        {
            UserModel user = isDocumentNumber
                ? await _repository.GetUserByDocumentNumberAsync(request.EmailOrDocumentNumber.ToLower())
                : await _repository.GetUserByEmailAsync(request.EmailOrDocumentNumber.ToLower());

            if (user == null || !PasswordService.VerifyPassword(user.password, request.Password))
            {
                if (user != null)
                {
                    user.attempts++;
                    if (user.attempts >= 3)
                    {
                        user.blocked = true;
                    }
                    await _repository.CommitAsync();

                    if (user.blocked)
                    {
                        throw new AuthenticationException("User is blocked. Please reset your password.");
                    }

                    throw new AuthenticationException($"Incorrect password! Number of attempts {user.attempts}/3");
                }
            }

            // Reset attempts after successful login
            if (user is not null && user.blocked)
                throw new AuthenticationException("User is blocked. Please reset your password.");

            if (user is null)
                throw new AuthenticationException("User not found.");

            user.attempts = 0;
            await _repository.CommitAsync();
            return user;
        }

        private string GenerateJwtToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.name),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("documentNumber", user.documentNumber)
            };

            // Add profile claims
            if (user.UserProfiles != null)
            {
                foreach (var userProfile in user.UserProfiles)
                {
                    claims.Add(new Claim("profile", userProfile.ProfileId.ToString()));
                }
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings["Issuer"],
                audience: _jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(double.Parse(_jwtSettings["ExpirationHours"])),
                signingCredentials: _credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
