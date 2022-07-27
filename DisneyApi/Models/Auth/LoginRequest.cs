using System.ComponentModel.DataAnnotations;

namespace DisneyApi.Models.Auth
{
    public class LoginRequest
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
