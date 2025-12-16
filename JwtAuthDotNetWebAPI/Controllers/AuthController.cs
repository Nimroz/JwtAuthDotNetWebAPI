using JwtAuthDotNetWebAPI.Entities;
using JwtAuthDotNetWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthDotNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        
        private static User user = new();

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.PasswordHash);

            user.UserName = request.UserName;
            user.PasswordHash = hashedPassword;

            return Ok(user);
        }
        [HttpPost("login")]
        public ActionResult<string> LogIn(UserDto request)
        {
            if(user.UserName != request.UserName)
            {
                return BadRequest("User Not Found");
            }
            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.PasswordHash) 
                == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong Pass");
            }
            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescryptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSetting: Issuer"),
                audience: configuration.GetValue<string>("AppSetting: Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescryptor);
        }
    }
}
