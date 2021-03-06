using System.ComponentModel.DataAnnotations;

namespace IS.Models.Dtos.User
{
    public class UserForLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
