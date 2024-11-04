using KhangNghi_BE.Data.Models;
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

        public async Task<User?> LoginAsync(string username, string password)
        {
            string? serverName = _configuration.GetSection("ServerName").Value;
            string? dbName = _configuration.GetSection("DbName").Value;

            try
            {
                string connectionString = $"Server={serverName};Database={dbName}" +
                                          $";User Id = {username}; Password = {password}" +
                                          $";TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    
                    User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                    return user;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<User?> RegisterAsync(string username, string password)
        {
            string? connectionString = _configuration.GetConnectionString("ConnectionString");
            string createLoginSql = $"Create Login [{username}] With Password = N'{password}'";
            string createUserSql = $"Create User [{username}] For Login [{username}]";

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
                    
                    User user = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Username = username,
                        CreateAt = DateTime.UtcNow,
                        RoleId = "user"
                    };

                    

                    return user;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<bool> ChangePasswordAsync(string userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken> CreateRefreshTokenAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
