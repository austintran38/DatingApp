using System.Security.Principal;
using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Create a WebApplication builder (sets up DI, configuration, logging, etc.)
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//register and use DbContext in application 
builder.Services.AddDbContext<AppDbContext>(opt =>
{//use Sqlite as database provider
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(); //allow secure and controlled access to your resources from different domains (origins), while still adhering to the browser's built-in security model, the same-origin policy
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        // Retrieve the secret key used to sign JWT tokens
        var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found - Program.cs");
        // Configure how incoming JWT tokens are validated
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Ensure the token is signed with a valid key
            ValidateIssuerSigningKey = true,
            // The secret key used to validate the token signature
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
// Allows requests from your Angular frontend running on localhost:4200
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().
    WithOrigins("http://localhost:4200","https://localhost:4200"));

//authentication middleware
app.UseAuthentication(); // This checks for JWT tokens in incoming requests
app.UseAuthorization(); // Enables authorization checks ([Authorize] attributes)

app.MapControllers();

app.Run();
