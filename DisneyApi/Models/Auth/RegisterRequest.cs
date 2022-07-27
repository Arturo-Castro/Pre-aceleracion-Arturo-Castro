using System.ComponentModel.DataAnnotations;

namespace DisneyApi.Models.Auth
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
