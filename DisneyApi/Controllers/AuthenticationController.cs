using DisneyApi.Models;
using DisneyApi.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using DisneyApi.Utilities;

namespace DisneyApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = $"User Creation Failed! Errors: {string.Join(", ", result.Errors.Select(x => x.Description))}"
                });
            }

            await Sendgrid.SendEmail(model.Email, model.Username);            

            return Ok(new
            {
                Status = "Success",
                Message = "User was created successfully!"
            });            
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(model.Username);

                if (currentUser.IsActive)
                {
                    return Ok(await GetToken(currentUser));
                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new
            {
                Status = "Error",
                Message = $"User {model.Username} is not authorized..."
            });
        }

        private async Task<LoginResponse> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, currentUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretLongAuthorizationKey"));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7106",
                audience: "https://localhost:7106",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }
}
