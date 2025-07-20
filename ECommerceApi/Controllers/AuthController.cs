using ECommerceApi.Models;
using ECommerceApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IRedisRepository _redisRepository;

        public AuthController(IUserRepository userRepository, IConfiguration configuration, IPasswordService passwordService, IRedisRepository redisRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordService = passwordService;
            _redisRepository = redisRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var result = _passwordService.VerifyPassword(user, user.PasswordHash, request.Password);
            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return BadRequest("No token provided");

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo - DateTime.UtcNow;

            if (expiry <= TimeSpan.Zero)
                return BadRequest("Token already expired");

            if(await _redisRepository.IsTokenBlacklistedAsync(token))
                return BadRequest("Already logged out");

            await _redisRepository.BlacklistTokenAsync(token, expiry);

            return Ok("Logged out");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
