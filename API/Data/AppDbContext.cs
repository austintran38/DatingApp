using API.Entities;
using Microsoft.EntityFrameworkCore;
/*
Entity Framework class
*/
namespace API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; } //a set of appuser
}
