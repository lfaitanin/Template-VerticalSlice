using Microsoft.AspNetCore.Identity;

namespace Shared.Services
{
    public static class PasswordService
    {
        private static PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

        public static string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
