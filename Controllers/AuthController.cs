using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PrimoAPITarjetas.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PrimoAPITarjetas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CardContext _context;

        public AuthController(IConfiguration configuration, CardContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == login.Username && u.Password == login.Password);

            if (user != null)
            {
                var token = GenerateJwtToken(user.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var keyString = jwtSection["Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new Exception("JWT Key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
