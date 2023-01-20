using OperationStacked.Abstractions;

namespace OperationStacked.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password) => 
            BCrypt.Net.BCrypt.HashPassword(password);
        
        public bool PasswordMatches(string providedPassword, string passwordHash) =>
            BCrypt.Net.BCrypt.Verify(providedPassword, passwordHash);

        }
}