namespace OperationStacked.Abstractions
{
    public interface IPasswordHasherService
    {
        public string HashPassword(string password);
        public bool PasswordMatches(string providedPassword, string passwordHash);
    }
}