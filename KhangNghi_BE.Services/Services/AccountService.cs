using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KhangNghi_BE.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly KhangNghiContext _context;
        private readonly IConfiguration _configuration;
        
        public AccountService(KhangNghiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User?> LoginAsync(Login login)
        {
            string? serverName = _configuration.GetSection("ServerName").Value;
            string? dbName = _configuration.GetSection("DbName").Value;

            try
            {
                string connectionString = $"Server={serverName};Database={dbName}" +
                                          $";User Id = {login.Username}; Password = {login.Password}" +
                                          $";TrustServerCertificate=True;Pooling=false;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    await connection.CloseAsync();
                }

                return await GetUserByUsernameAsync(login.Username);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<User?> RegisterAsync(Register register)
        {
            string? connectionString = _configuration.GetSection("ConnectionString").Value;
            string createLoginSql = $"Create Login [{register.Username}] With Password = N'{register.Password}'";
            string createUserSql = $"Create User [{register.Username}] For Login [{register.Username}]";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(createLoginSql, connection))
                    {
                       await cmd.ExecuteNonQueryAsync();
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(createUserSql, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    
                    await connection.CloseAsync();

                    User user = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Username = register.Username,
                        CreateAt = DateTime.UtcNow,
                        RoleId = "user",
                        Email = register.Email,
                        IsActive = true,
                    };

                    await _context.Users.AddAsync(user);
                    bool result = await _context.SaveChangesAsync() > 0;
                    
                    return result ? user : null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == refreshToken); 
        }

        public async Task<RefreshToken?> CreateRefreshTokenAsync(string userId, string token, string jwtId)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                CreateAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(30),
                IsUsed = false,
                JwtId = jwtId
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            bool result = await _context.SaveChangesAsync() > 0;
            
            return result ? refreshToken : null;
        }

        public async Task<bool> UpdateRefreshTokenAsync(string refreshToken)
        {
            RefreshToken? token = await GetRefreshTokenAsync(refreshToken);
            if(token != null) 
            {
                token.IsUsed = true;
                token.IsRevoked = true;

                _context.RefreshTokens.Update(token);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;         
        }

        public async Task<bool> RevokeRefreshTokens(string userId)
        {
            IEnumerable<RefreshToken> tokens = await _context.RefreshTokens
                .Where(r => r.UserId == userId)
                .ToListAsync();

            foreach (RefreshToken token in tokens)
            {
                token.IsRevoked = true;
            }

            _context.UpdateRange(tokens);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User?> GetUserAsync(string userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> ChangePasswordAsync(string userId, ChangePassword password)
        {
            User? user = await GetUserAsync(userId);
            if (user == null) {
                return false;
            }

            if(await LoginAsync(new Login { Username = user.Username, Password = password.OldPassword}) == null)
            {
                return false;
            }

            string? connectionString = _configuration.GetSection("ConnectionString").Value;
            string changePasswordSql = $"Alter Login [{user.Username}] With Password = N'{password.NewPassword}'";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(changePasswordSql, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
 
                    await connection.CloseAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
