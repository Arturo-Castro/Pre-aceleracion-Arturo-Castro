using Microsoft.AspNetCore.Identity;

namespace DisneyApi.Models
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; }
    }
}
