using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KhangNghi_BE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    
    public AccountController(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        User? user = await _accountService.LoginAsync(username, password);

        if (user == null)
        {
            return BadRequest(new ApiReponse
            {
                Success = false,
                Message = "Invalid username or password"
            });
        }
        
        return Ok(new ApiReponse
        {
            Success = true,
            Data = new
            {
                user = user,
                accessToken = GenerateToken(user),
                refreshToken = GenerateRefreshToken()
            }
        });
    }

    private string? GenerateToken(User user)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        
        string? secretKey = _configuration.GetSection("SecretKey").Value;
        if (secretKey != null)
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.RoleId),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }

        return null;
    }
    
    private string GenerateRefreshToken()
    {
        byte[] bytes = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}