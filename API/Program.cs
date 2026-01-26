using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//register and use DbContext in application 
builder.Services.AddDbContext<AppDbContext>(opt =>
{//use Sqlite as database provider
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(); //allow secure and controlled access to your resources from different domains (origins), while still adhering to the browser's built-in security model, the same-origin policy

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().
WithOrigins("http://localhost:4200","https://localhost:4200"));

app.MapControllers();

app.Run();
