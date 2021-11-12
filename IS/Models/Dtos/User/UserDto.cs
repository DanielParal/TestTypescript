using System;

namespace IS.Models.Dtos.User
{
    public class UserDto
    {
        public string Guid { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
