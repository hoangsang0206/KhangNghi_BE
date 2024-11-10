using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Services;
using KhangNghi_BE.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KhangNghi_BE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    
    private readonly string? _secretKey;
    
    public AccountController(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
        _secretKey = _configuration.GetSection("SecretKey").Value;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        if(ModelState.IsValid)
        {
            User? user = await _accountService.LoginAsync(login);

            if (user == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sai tên đăng nhập hoặc mật khẩu"
                });
            }

            await _accountService.UpdateRefreshTokenAsync(user.UserId);

            Token? token = await GenerateToken(user);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = token
            });
        }

        return BadRequest(new ApiResponse
        {
            Success = false,
            Message = "Dữ liệu không hợp lệ",
            Data = ModelState
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register register)
    {
        if (ModelState.IsValid)
        {
            User? existedUser = await _accountService.GetUserByUsernameAsync(register.Username);
            if(existedUser != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Tên đăng nhập đã tồn tại"
                });
            }
            
            User? user = await _accountService.RegisterAsync(register);

            if (user == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Đăng ký thất bại, đã xảy ra lỗi"
                });
            }

            Token? token = await GenerateToken(user);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = token
            });
        }
        
        return BadRequest(new ApiResponse
        {
            Success = false,
            Message = "Dữ liệu không hợp lệ",
            Data = ModelState
        });
    }



    [HttpPost("renew-token")]
    public async Task<IActionResult> RenewToken([FromBody] RenewToken rToken)
    {
        try
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_secretKey ?? "");

            var tokenValidateParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };

            var tokenVerification = handler.ValidateToken(rToken.AccessToken, tokenValidateParams, out SecurityToken validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Token không hợp lệ"
                    });
                }
            }

            var utcExpireDate = long.Parse(tokenVerification.Claims
                .FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Exp)?.Value ?? "");

            if(utcExpireDate.ConvertUnixTimeToDateTime() > DateTime.UtcNow)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Token chưa hết hạn"
                });
            }


            RefreshToken? token = await _accountService.GetRefreshTokenAsync(rToken.RefreshToken);

            if (token == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Token không hợp lệ"
                });
            }

            if (token.IsUsed == true)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Token đã được sử dụng"
                });
            }

            if(token.IsRevoked == true)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Token đã bị thu hồi"
                });
            }

            if (token.ExpireAt <= DateTime.UtcNow)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Token đã hết hạn, vui lòng đăng nhập lại"
                });
            }

            var jti = tokenVerification.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jti != token.JwtId)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Token không hợp lệ"
                });
            }

            await _accountService.UpdateRefreshTokenAsync(rToken.RefreshToken);
            User? user = await _accountService.GetUserAsync(token.UserId);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = user != null ? await GenerateToken(user) : null
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Đã xảy ra lỗi"
            });
        }
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePassword password)
    {
        if(ModelState.IsValid)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy thông tin người dùng"
                });
            }

            bool result = await _accountService.ChangePasswordAsync(userId, password);

            return Ok(new ApiResponse
            {
                Success = result,
                Message = result ? "Đổi mật khẩu thành công" : "Đổi mật khẩu thất bại"
            });
        }

        return BadRequest(new ApiResponse
        {
            Success = false,
            Message = "Dữ liệu không hợp lệ",
            Data = ModelState
        });
    }

    private async Task<Token?> GenerateToken(User user)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        
        if (_secretKey != null)
        {
            byte[] key = Encoding.UTF8.GetBytes(_secretKey);
            string tokenId = Guid.NewGuid().ToString();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.RoleId),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            string accessToken = handler.WriteToken(handler.CreateToken(descriptor));
            string refreshToken = GenerateRefreshToken();
            Token token = new Token
            {
                TokenId = tokenId,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            
            await _accountService.CreateRefreshTokenAsync(user.UserId, refreshToken, token.TokenId);

            return token;
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