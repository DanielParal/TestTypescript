namespace IS.Services.Authorization
{
    public interface IPasswordService
    {
        public string HashPassword(string password, byte[] salt = null, bool needsOnlyHash = false);
        public bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck);
    }
}
