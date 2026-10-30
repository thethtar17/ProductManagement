using management.Data;
using management.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace management.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ClaimsPrincipal _user;
        public UserController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _user = httpContextAccessor.HttpContext.User;
        }

        [Authorize]
        [HttpGet("current-user")]
        public IActionResult GetCurrentUser()
        {
            var userId = _user.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = _user.FindFirstValue(ClaimTypes.Name);
            var userIds = User.FindFirstValue("userId");

            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(new { userId, username,userIds });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Role))
            {
                request.Role = "USER"; // Assign default role
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully!" });
        }


        
        [HttpGet("protected-resource")]
        public IActionResult GetProtectedResource()
        {
            return Ok(new { message = "You have access to this protected resource!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Email and Password are required." });
            }

            // Find the user by email
            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) // Compare using BCrypt
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            // Generate a JWT token if credentials are valid
            var token = GenerateJwtToken(user);

            return Ok(new { token, user = new { user.Username, user.Email,user.Id } });
        }

    
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Include the user's ID
        new Claim("role", user.Role), // Dynamically include the user's role
         new Claim("email", user.Email),
         new Claim("userId", user.Id.ToString())



    };

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: creds,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
