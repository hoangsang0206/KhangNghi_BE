using System.Text;
using System.Text.Json.Serialization;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Services;
using KhangNghi_BE.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"] ?? "")),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add CORS policy
builder.Services.AddCors(options =>
{
    string[]? allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        if(allowedOrigins != null && allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

builder.Services.AddDbContext<KhangNghiContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value);
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IServiceCatalogService, ServiceCalalogService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowSpecificOrigins");

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions 
{ 
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
    RequestPath = "/Files"
});

using (IServiceScope scope = app.Services.CreateScope())
{
    KhangNghiContext context = scope.ServiceProvider.GetRequiredService<KhangNghiContext>();

    if (!await context.Roles.AnyAsync(r => r.RoleId == "user"))
    {
        await context.Roles.AddAsync(new Role { RoleId = "user", RoleName = "User" });
        await context.SaveChangesAsync();
    }

    if (!await context.Roles.AnyAsync(r => r.RoleId == "admin"))
    {
        await context.Roles.AddAsync(new Role { RoleId = "admin", RoleName = "Admin" });
        await context.SaveChangesAsync();
    }

    if (!await context.UserGroups.AnyAsync(g => g.GroupId == "admin"))
    {
        await context.UserGroups.AddAsync(new UserGroup { GroupId = "admin", GroupName = "Admin", HasAllPermission = true });
        await context.SaveChangesAsync();
    }

    IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

    if(await accountService.GetUserByUsernameAsync("admin") == null)
    {
        await accountService.CreateUserAsync(new User
        {
            UserId = Guid.NewGuid().ToString(),
            Username = "admin",
            CreateAt = DateTime.UtcNow,
            RoleId = "admin",
            Email = "admin@khangnghi.com",
            IsActive = true,
            GroupId = "admin"
        }, "admin@123");
    }
}

app.Run();